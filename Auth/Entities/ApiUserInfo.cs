using System;

namespace Auth.Entities {
    public class ApiUserInfo {
        // 인증키 부여 구현 방법
        // - 인증키 값
        // - 서비스 명
        // - 서비스 목적
        // - 서비스 URL 주소
        // - 제공자(회사)명
        // - 제공자 연락처(대표전화전화)
        // - 최초 발급일자
        // - 허가자
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string ServicePurpose { get; set; }
        public string ServiceUrl { get; set; }
        public string UserName { get; set; }
        public string UserContact { get; set; }
        public DateTime CreateDate { get; set; }
        public string Authorizer { get; set; }

    }
}