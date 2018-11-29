using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services {
    public interface ITokenService {
        TokenInfo GetToken (User user, ApiUserInfo apiUserInfo);
    }
    public class TestTokenService : ITokenService {

        private readonly AppSettings _appSettings;
        public TestTokenService (IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public TokenInfo GetToken (User user, ApiUserInfo apiUserInfo) {

            TokenInfo tokenInfo = new TokenInfo ();

            tokenInfo.JwtToken = this.CreatJwtToken (user, apiUserInfo);
            tokenInfo.UserId = user.Id;
            tokenInfo.UserName = user.UserName;

            return tokenInfo;

        }

        private string CreatJwtToken (User user, ApiUserInfo apiUserInfo) {

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_appSettings.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                // 클레임 생성
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.Name, user.Id.ToString ())
                }),
                Expires = DateTime.UtcNow.AddMinutes (_appSettings.JwtExpireMins),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken (token);
            return jwtToken;
        }
    }
}