namespace MovieAPI.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
