namespace Domain.Chats
{
    public class Message
    {
        public Guid Id { get; set; }
        public UserId User { get; set; }
        public RoomId Room { get; set; }
        public DateTime DtInserted { get; set; }
    }
}
