using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public interface ILoginService
    {
        User Login(string id, string password);
    }
    public class TestLoginService : ILoginService
    {

        private static List<User> users = new List<User>();

        public TestLoginService()
        {
            users.Add(new User() { isAuth = true, Id = "1", UserName = "test1" });
            users.Add(new User() { isAuth = true, Id = "2", UserName = "test2" });
            users.Add(new User() { isAuth = true, Id = "3", UserName = "test3" });

        }

        public User Login(string id, string password)
        {
            var user = users.Find(x => x.Id == id);

            if (user == null)
            {
                throw new System.Exception("User Not Found");
            }

            return user;
        }
    }
}