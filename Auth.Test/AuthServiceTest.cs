using System;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Test
{
    public class AuthServiceTest : IDisposable
    {

        private ILoginService loginService;
        private IApiKeyValiationService apiUserValidService;
        private BaseTokenService tokenService;
        private IDapperRepository repo;

        public AuthServiceTest()
        {
            var appsetting = Options.Create(new AppSettings()
            {
                JwtIssuer = "https://apis.donga.ac.kr/auth",
                JwtExpireMins = 15,
                JwtKey = "ECOMMERCE_SUPER_SECRET_KEY"
            });

            loginService = new TestLoginService();
            apiUserValidService = new TestApiKeyValiationService();
            repo = new BaseRepository(new SqliteRepositoryString(new BaseORMHelper()));
            tokenService = new BaseTokenService(appsetting);

        }

        [Fact]
        public void 로그인_서비스_실행()
        {

            string expect = "test1";

            // User user = loginService.Login("1", "2222");
            User user = loginService.Login("1", "2222");

            Assert.Equal(expect, user.UserName);
        }

        [Fact]
        public void apiKey유저유효확인_서비스_실행()
        {

            string expect = "https://test1.donga.ac.kr";

            var apiUserInfo = apiUserValidService.Validate("apikey1");

            Assert.Equal(expect, apiUserInfo.ServiceUrl);
        }

        [Fact]
        public void 인증토큰발행_서비스_실행()
        {

            User user = loginService.Login("1", "2222");

            ApiUserInfo apiUserInfo = apiUserValidService.Validate("apikey1");


            var tokenInfo = tokenService.GetToken(user, apiUserInfo);

            string oldjwtToken = tokenInfo.JwtToken;

            string oldRefreshToken = tokenInfo.RefreshToken;

            var oldPrincipal = tokenService.GetPrincipalFromExpiredToken(oldjwtToken);



            Assert.Equal("test1", oldPrincipal.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            Assert.Equal("1", oldPrincipal.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value);
            Assert.Equal("https://apis.donga.ac.kr/auth", oldPrincipal.FindFirst(x => x.Type == "iss").Value);
            Assert.Equal("https://test1.donga.ac.kr", oldPrincipal.FindFirst(x => x.Type == "aud").Value);

            // 잘못된 RefreshToken 값을 넣으면 예외 Throw..
            Assert.Throws<SecurityTokenException>(() => tokenService.RefreshToken(oldjwtToken, "WrongRefreshToken"));



            var newTokenInfo = tokenService.RefreshToken(oldjwtToken, oldRefreshToken);


            string newJwtToken = newTokenInfo.JwtToken;
            string newRefreshToken = newTokenInfo.RefreshToken;


            Assert.NotEqual(oldjwtToken, newJwtToken);

            Assert.NotEqual(oldRefreshToken, newRefreshToken);

            var newPrincipal = tokenService.GetPrincipalFromExpiredToken(newJwtToken);

            Assert.Equal("test1", newPrincipal.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            Assert.Equal("1", newPrincipal.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value);
            Assert.Equal("https://apis.donga.ac.kr/auth", newPrincipal.FindFirst(x => x.Type == "iss").Value);
            Assert.Equal("https://test1.donga.ac.kr", newPrincipal.FindFirst(x => x.Type == "aud").Value);

        }


        // 테스트가 끝나면 리소스 반납
        public void Dispose()
        {

        }


    }
}