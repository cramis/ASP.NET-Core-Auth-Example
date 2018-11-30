using Auth.Entities;
using Microsoft.Extensions.Options;

namespace Auth.Services
{

    public interface ITest
    {
        string GetVal();
    }
    public class Test : ITest
    {
        private readonly AppSettings _settings;
        public Test(IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
        }

        public string GetVal()
        {
            return _settings.JwtKey;
        }
    }
}