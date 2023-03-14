namespace Chat.Bot.Consumer.Models
{
    public class QuoteMessage
    {
        public Guid MessageId { get; set; }
        public UserModel User { get; set; }
        public Guid RoomId { get; set; }
        public string Text { get; set; }
        public DateTime DtInserted { get; set; }
    }
}
