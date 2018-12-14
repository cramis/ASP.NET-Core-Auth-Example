using System;
using System.Collections.Generic;
using Auth.Entities;
using DapperRepository;

namespace Auth.Services
{
    public interface IApiKeyValiationService
    {
        ApiUserInfo Validate(string apiKey);
    }
    public class TestApiKeyValiationService : IApiKeyValiationService
    {

        private static List<ApiUserInfo> infos = new List<ApiUserInfo>();

        public TestApiKeyValiationService()
        {
            infos.Add(new ApiUserInfo()
            {
                Id = 1,
                ApiKey = "apikey1",
                ServiceName = "서비스1",
                ServicePurpose = "서비스목적1",
                ServiceUrl = "https://test1.donga.ac.kr",
                UserName = "사용자1",
                UserContact = "010-1234-5678",
                CreateDate = new DateTime(2018, 11, 29),
                Authorizer = "허가자1"
            });

            infos.Add(new ApiUserInfo()
            {
                Id = 2,
                ApiKey = "apikey2",
                ServiceName = "서비스2",
                ServicePurpose = "서비스목적2",
                ServiceUrl = "https://test2.donga.ac.kr",
                UserName = "사용자2",
                UserContact = "010-1234-5678",
                CreateDate = new DateTime(2018, 11, 29),
                Authorizer = "허가자2"
            });

            infos.Add(new ApiUserInfo()
            {
                Id = 3,
                ApiKey = "apikey3",
                ServiceName = "서비스3",
                ServicePurpose = "서비스목적3",
                ServiceUrl = "https://test3.donga.ac.kr",
                UserName = "사용자3",
                UserContact = "010-1234-5678",
                CreateDate = new DateTime(2018, 11, 29),
                Authorizer = "허가자3"
            });
        }
        public ApiUserInfo Validate(string apiKey)
        {

            var apiUser = infos.Find(x => x.ApiKey == apiKey);

            if (apiUser == null)
            {
                throw new System.Exception("apiKey Not Valid");
            }

            return apiUser;
        }
    }

    public class ORMApiKeyValiationService : IApiKeyValiationService
    {
        public IDapperRepository repo { get; }

        public ORMApiKeyValiationService(IDapperRepository repo)
        {
            this.repo = repo;
        }
        public ApiUserInfo Validate(string apiKey)
        {

            var apiUser = this.repo.GetItem(new ApiUserInfo() { ApiKey = apiKey });

            if (apiUser == null)
            {
                throw new System.Exception("apiKey Not Valid");
            }

            return apiUser;
        }
    }
}