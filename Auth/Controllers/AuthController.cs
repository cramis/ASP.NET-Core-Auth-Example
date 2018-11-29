using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IApiKeyValiationService apikeyValidService;
        private readonly ITokenService tokenService;

        public AuthController(ILoginService loginService, IApiKeyValiationService apikeyValidService, ITokenService tokenService)
        {
            this.loginService = loginService;
            this.apikeyValidService = apikeyValidService;
            this.tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginInfo loginInfo)
        {

            try
            {
                TokenInfo tokenInfo = new TokenInfo();

                var apiUserInfo = this.apikeyValidService.Validate(loginInfo.APIKey);

                if (apiUserInfo == null)
                {
                    return NotFound("유효한 apiKey가 아닙니다.");
                }

                var user = this.loginService.Login(loginInfo.Id, loginInfo.Password);

                if (user == null)
                {
                    return NotFound("아이디 또는 패스워드가 올바르지 않습니다.");
                }

                tokenInfo = this.tokenService.GetToken(user, apiUserInfo);

                if (tokenInfo == null)
                {
                    return NotFound("토큰 발급에 실패했습니다.");
                }

                return Ok(tokenInfo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }

        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

    }
}