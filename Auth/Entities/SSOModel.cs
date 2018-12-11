using System;

namespace Auth.Entities
{
    public class SSOResult
    {
        public bool ok { get; set; }
        public string Message { get; set; }
        public string SsoKey { get; set; }
        public bool isLogined { get; set; }
        public string LoginCallerIP { get; set; }
        public string LoginSite { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginTime { get; set; }
        public string TTL { get; set; }

        public LoginUser loginUser { get; set; }

    }

    public class LoginUser
    {
        public int UserType { get; set; }
        public string EtcUserType { get; set; }
        public string UserID { get; set; }
        public string EmpNo { get; set; }
        public string ADAccount { get; set; }
        public string UserName { get; set; }
        public string UserName_Eng { get; set; }
        public string DeptCD { get; set; }
        public string DeptName { get; set; }
        public string[] SecondDeptCD { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool EmailReceiveYn { get; set; }
        public string Sex { get; set; }
        public string RootDeptCD { get; set; }
        public string KindOfWork { get; set; }
        public string Position { get; set; }
        public string Emp_Status { get; set; }
        public string ADAccountLockStatus { get; set; }
        public string HP { get; set; }
    }
}