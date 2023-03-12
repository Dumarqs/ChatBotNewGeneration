namespace Domain.Chats
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public User User { get; set; }
        public Guid RoomId { get; set; }
        public string Text { get; set; }
        public DateTime DtInserted { get; set; }
    }
}
