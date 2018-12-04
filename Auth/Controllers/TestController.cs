using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Entities;
using Auth.Services;
using DapperRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auth.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IDapperRepository repo;

        public TestController(IDapperRepository repo)
        {
            this.repo = repo;
            this.repo.SetConnection(new ConnectionFactory().Connection("sqlite"));
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var list = this.repo.GetList(new TestModel());
            return Ok(list);

        }
    }
}