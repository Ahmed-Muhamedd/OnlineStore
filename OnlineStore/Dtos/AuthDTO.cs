namespace OnlineStore.Dtos
{
    public class AuthDTO
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<string> Message { get; set; } = null!;
    }
}
