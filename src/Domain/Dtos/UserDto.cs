namespace Domain.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
