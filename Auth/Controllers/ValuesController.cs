using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Entities;
using Auth.Services;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auth.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController()
        {

        }

        [HttpGet("flurl")]
        public async Task<object> Flurl()
        {
            //     id: "cramis",
            //      password: "1111",
            //      apikey: "dsfjkv43hcxcvjnm4fbj"
            var result = await "http://localhost:5000/api/auth"
            .PostJsonAsync(new
            {
                id = "cramis",
                password = "1111",
                apikey = "dsfjkv43hcxcvjnm4fbj"
            })
            .ReceiveJson<TokenInfo>();




            return result;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var Name = User.Identity.Name;
            var authenticationType = User.Identity.AuthenticationType;
            var isAuthenticated = User.Identity.IsAuthenticated;
            User.Claims.Select(x => x.Subject);



            return new string[] { "value1", "value2", Name, authenticationType, isAuthenticated.ToString() };

        }



        [Authorize]
        [HttpGet("name")]
        public object Nameidentifier()
        {
            return User.Claims.SingleOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) { }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}