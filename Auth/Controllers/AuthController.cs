using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auth.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly ILoginService loginService;
        private readonly IApiKeyValiationService apiketValidService;
        private readonly ITokenService tokenService;

        public AuthController (ILoginService loginService, IApiKeyValiationService apiketValidService, ITokenService tokenService) {
            this.loginService = loginService;
            this.apiketValidService = apiketValidService;
            this.tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Post ([FromBody] LoginInfo loginInfo) {
            TokenInfo tokenInfo = new TokenInfo ();
            tokenInfo.JwtToken = "1111";
            tokenInfo.UserId = loginInfo.Id;
            tokenInfo.UserName = loginInfo.APIKey;
            return Ok (tokenInfo);
        }

    }
}