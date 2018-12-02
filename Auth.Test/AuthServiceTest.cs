using System;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Xunit;

using DapperRepository;

namespace Auth.Test
{
    public class AuthServiceTest : IDisposable
    {

        private ILoginService loginService;
        private IApiKeyValiationService apiUserValidService;
        private ITokenService tokenService;

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
            tokenService = new TestTokenService(appsetting);

        }

        [Fact]
        public void 로그인_서비스_실행()
        {

            string expect = "test1";

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

        // [Fact]
        // public void 인증토큰발행_서비스_실행()
        // {

        //     User user = loginService.Login("1", "2222");
        //     ApiUserInfo apiUserInfo = apiUserValidService.Validate("apikey1");

        //     string expect = "https://apis.donga.ac.kr/auth";

        //     var tokenInfo = tokenService.GetToken(user, apiUserInfo);

        //     Assert.Equal(expect, tokenInfo.JwtToken);

        // }


        // 테스트가 끝나면 리소스 반납
        public void Dispose()
        {

        }


    }
}