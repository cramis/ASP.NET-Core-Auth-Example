using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Entities;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IApiKeyValiationService apikeyValidService;
        private readonly ITokenService tokenService;
        private readonly Serilog.ILogger logger;

        public JwtController(ILoginService loginService, IApiKeyValiationService apikeyValidService, ITokenService tokenService)
        {
            this.loginService = loginService;
            this.apikeyValidService = apikeyValidService;
            this.tokenService = tokenService;
            this.logger = Log.Logger;
        }

        [HttpPost("authentication")]
        public IActionResult Authentication([FromBody] LoginInfo loginInfo)
        {

            try
            {
                TokenInfo tokenInfo = new TokenInfo();

                var apiUserInfo = this.apikeyValidService.Validate(loginInfo.APIKey);


                logger.Debug("apiUserInfo : {apiUserInfo}", JsonConvert.SerializeObject(apiUserInfo));

                if (apiUserInfo == null)
                {
                    return NotFound("유효한 apiKey가 아닙니다.");
                }

                var user = this.loginService.Login(loginInfo.Id, loginInfo.Password);

                logger.Debug("user : {@user}", JsonConvert.SerializeObject(user));

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
                logger.Error(ex, "인증에 실패했습니다.");
                return NotFound("인증에 실패했습니다.");
            }

        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenViewModel refreshToken)
        {
            try
            {
                var tokenInfo = this.tokenService.RefreshToken(refreshToken.JwtToken, refreshToken.RefreshToken);

                if (tokenInfo == null)
                {
                    return NotFound("토큰 재발급에 실패했습니다.");
                }

                return Ok(tokenInfo);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "인증에 실패했습니다.");
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ValidateToken")]
        public IActionResult ValidateToken(string Userid, string token)
        {
            try
            {
                var isValid = this.tokenService.ValidateToken(Userid, token);

                return Ok(isValid);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "토큰 검증에 실패했습니다.");
                return NotFound(ex.Message);
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