using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Config;
using Auth.Entities;
using DapperRepository;
using Flurl;
using Flurl.Http;
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

    public class SSOApiLoginService : ILoginService
    {
        public User Login(string id, string password)
        {
            return AsyncHelper.RunSync<User>(() => this.SSOLogin(id, password));
        }

        private async Task<User> SSOLogin(string id, string password)
        {
            var ipAddress = SSOApiConfig.Ip;

            var token = await SSOApiConfig.GetToken
            .PostUrlEncodedAsync(new
            {
                AccessKey = SSOApiConfig.AccessKey,
                CallerSite = SSOApiConfig.CallerSite,
                ClientIP = ipAddress
            }).ReceiveJson<string>();

            var ssoResult = await SSOApiConfig.EmpLogin
            .PostUrlEncodedAsync(new
            {
                Token = token,
                AccessKey = SSOApiConfig.AccessKey,
                CallerSite = SSOApiConfig.CallerSite,
                EmpNoOrAccount = id,
                Password = password,
                ClientIP = ipAddress
            }).ReceiveJson<SSOResult>();

            var result = await SSOApiConfig.GetUserinfo
            .PostUrlEncodedAsync(new
            {
                Token = token,
                AccessKey = SSOApiConfig.AccessKey,
                CallerSite = SSOApiConfig.CallerSite,
                ssokey = ssoResult.Message,
                ClientIP = ipAddress
            }).ReceiveJson<SSOResult>();

            User user = new User();

            user.isAuth = true;
            user.Id = result.loginUser.UserID;
            user.UserName = result.loginUser.UserName;

            return user;
        }
    }
}