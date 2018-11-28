namespace Auth.Entities
{
    public class User
    {
        public bool isAuth { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}