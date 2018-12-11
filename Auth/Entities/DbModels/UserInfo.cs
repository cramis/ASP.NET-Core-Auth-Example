using DapperRepository;

namespace Auth.Entities
{
    [Table("User")]
    public class UserInfo
    {
        [KeyColumn]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}