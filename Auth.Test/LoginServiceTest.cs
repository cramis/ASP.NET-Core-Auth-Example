using System;
using Xunit;


using Auth.Services;
using Auth.Entities;

namespace Auth.Test
{
    public class LoginServiceTest
    {
        [Fact]
        public void Test1()
        {
            TestLoginService service = new TestLoginService();


            string expect = "test1";

            User user = service.Authenticate("1", "2222", "test");

            Assert.Equal(expect, user.UserName);



        }
    }
}
