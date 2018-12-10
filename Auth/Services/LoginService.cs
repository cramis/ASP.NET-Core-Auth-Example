using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Entities;
using DapperRepository;
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

    public class SqliteLoginService : ILoginService
    {

        public IDapperRepository repo { get; }

        public SqliteLoginService(IDapperRepository repo)
        {
            this.repo = repo;
            this.repo.SetConnection(new ConnectionFactory().Connection("sqlite"));
        }

        public User Login(string id, string password)
        {

            UserInfo userInfo = new UserInfo();
            User user = new User();

            user.isAuth = false;

            userInfo.Id = id;

            var loginUser = this.repo.GetItem(userInfo);

            if (loginUser == null)
            {
                throw new System.Exception("User Not Found");
            }

            if (loginUser.Password != password)
            {
                throw new System.Exception("Password is Not Correnct.");
            }


            user.Id = loginUser.Id;
            user.UserName = loginUser.UserName;
            user.isAuth = true;

            return user;
        }
    }
}