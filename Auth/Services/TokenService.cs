using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public interface ITokenService
    {
        TokenInfo GetToken(User user, ApiUserInfo apiUserInfo);

        string CreateRefreshToken();
    }
    public class TestTokenService : ITokenService
    {

        private readonly AppSettings _appSettings;
        public TestTokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public TokenInfo GetToken(User user, ApiUserInfo apiUserInfo)
        {

            try
            {
                TokenInfo tokenInfo = new TokenInfo();

                tokenInfo.JwtToken = this.CreatJwtToken(user, apiUserInfo);
                tokenInfo.UserId = user.Id;
                tokenInfo.UserName = user.UserName;
                tokenInfo.RefreshToken = this.CreateRefreshToken();

                return tokenInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string CreateRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return refreshToken;
        }

        private string CreatJwtToken(User user, ApiUserInfo apiUserInfo)
        {

            // return null if user not found
            if (user == null)
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
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtExpireMins);

            var token = new JwtSecurityToken(
              _appSettings.JwtIssuer,
              apiUserInfo.ServiceUrl,
              claims,
              expires: expires,
              signingCredentials: creds
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

    }
}