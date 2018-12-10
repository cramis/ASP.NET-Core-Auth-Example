using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Auth.Services
{
    public interface ITokenService
    {
        TokenInfo GetToken(User user, ApiUserInfo apiUserInfo);

        TokenInfo RefreshToken(string token, string refreshToken);


    }


    public class TestTokenService : ITokenService
    {

        public string __RefreshToken { get; set; }

        public DateTime NowDate { get; set; }

        public string Jti { get; set; }

        private readonly AppSettings _appSettings;
        public TestTokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            this.NowDate = DateTime.UtcNow;
            this.Jti = Guid.NewGuid().ToString();
        }

        public TokenInfo GetToken(User user, ApiUserInfo apiUserInfo)
        {

            try
            {
                TokenInfo tokenInfo = new TokenInfo();

                tokenInfo.JwtToken = this.CreatJwtToken(user.Id, user.UserName, apiUserInfo.ServiceUrl);
                tokenInfo.UserId = user.Id;
                tokenInfo.UserName = user.UserName;
                tokenInfo.RefreshToken = this.CreateRefreshToken();

                // RefreshToken 업데이트
                this.UpdateRefreshToken(user.Id, user.UserName, tokenInfo.RefreshToken);

                return tokenInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public TokenInfo RefreshToken(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);

            // 토큰 값에서 해당하는 id, userName, audience를 가지고 옴
            var id = principal.Identity.Name;
            var userName = principal.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var audience = principal.FindFirst(x => x.Type == "aud")?.Value;

            var savedRefreshToken = this.GetRefreshToken(id, audience); //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = this.CreatJwtToken(id, userName, audience);
            var newRefreshToken = this.CreateRefreshToken();
            this.UpdateRefreshToken(id, audience, newRefreshToken);

            TokenInfo tokenInfo = new TokenInfo();

            tokenInfo.JwtToken = newJwtToken;
            tokenInfo.UserId = id;
            tokenInfo.UserName = userName;
            tokenInfo.RefreshToken = newRefreshToken;

            return tokenInfo;


        }

        private void UpdateRefreshToken(string id, string audience, string refreshToken)
        {
            // 저장되어 있는 RefreshToken을 대체한다.
            __RefreshToken = refreshToken;
        }

        private string GetRefreshToken(string id, string audience)
        {
            // 저장되어 있는 refreshToken을 가지고 옴
            return __RefreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }

        private string CreatJwtToken(string id, string userName, string audience)
        {

            // return null if user not found
            if (string.IsNullOrWhiteSpace(id))
                return null;

            // - JWT Token : 인증용 토큰
            // 1. Iss : JWT 인증을 해주고자 하는 인증 서비스 ( [https://apis.donga.ac.kr/auth](https://apis.donga.ac.kr/auth) )
            // 2. Aud : JWT 인증받고자 하는 서비스명( 학생정보서비스인 경우 [https://student.donga.ac.kr](https://student.donga.ac.kr) )
            // 3. Sub : 사용자 ID
            // 4. Name : 사용자 성명
            // 5. UserDiv : 학생/교직원/기타 대상자 여부
            // 6. Jti : 토큰 고유번호 
            // 7. Iat: 토큰 발급시간 
            // 8. Exp : 토큰 만료일자 - refresh 값을 이용해 갱신 가능 ( 디폴트 : 발급 후 10분 )
            // 9. Des : 토큰 최종 만료 일자 ( 디폴트 : 3일 )

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(ClaimTypes.Name, id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtExpireMins);

            var token = new JwtSecurityToken(
              _appSettings.JwtIssuer,
              audience,
              claims,
              expires: expires,
              signingCredentials: creds
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

    }


}