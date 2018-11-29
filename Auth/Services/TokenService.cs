using Auth.Entities;
using Microsoft.Extensions.Options;

namespace Auth.Services {
    public interface ITokenService {
        TokenInfo CreateToken (User user, ApiUserInfo apiUserInfo);
    }
    public class TestTokenService : ITokenService {

        private readonly AppSettings _appSettings;
        public TestTokenService (IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public TokenInfo CreateToken (User user, ApiUserInfo apiUserInfo) {

            TokenInfo tokenInfo = new TokenInfo ();

            tokenInfo.JwtToken = _appSettings.JwtIssuer;
            tokenInfo.UserId = user.Id;
            tokenInfo.UserName = user.UserName;

            return tokenInfo;

        }
    }
}