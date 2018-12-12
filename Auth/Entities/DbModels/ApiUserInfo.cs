using System;
using DapperRepository;

namespace Auth.Entities
{
    [Table("ApiUserInfo")]
    public class ApiUserInfo
    {
        // 인증키 부여 구현 방법
        // - 인증키 값
        // - 서비스 명
        // - 서비스 목적
        // - 서비스 URL 주소
        // - 제공자(회사)명
        // - 제공자 연락처(대표전화전화)
        // - 최초 발급일자
        // - 허가자

        [KeyColumn]
        public int? Id { get; set; }
        [RequiredColumn]
        public string ApiKey { get; set; }
        public string ServiceName { get; set; }
        public string ServicePurpose { get; set; }
        public string ServiceUrl { get; set; }
        public string UserName { get; set; }
        public string UserContact { get; set; }

        [CreatedDate]
        public DateTime? CreateDate { get; set; }
        public string Authorizer { get; set; }

    }
}