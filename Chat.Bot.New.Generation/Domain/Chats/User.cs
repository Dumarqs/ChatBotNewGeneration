namespace Domain.Chats
{
    public class User
    {
        public UserId UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }
    }
}
