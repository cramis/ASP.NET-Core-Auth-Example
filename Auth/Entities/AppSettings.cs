namespace Auth.Entities {

    public class AppSettings {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpireMins { get; set; }

    }
}