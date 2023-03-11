namespace Chat.Bot.API.Models
{
    public class MessageViewModel
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public string Text { get; set; }
        public DateTime DtInserted { get; set; }

        public bool IsCommand()
        {
            return Text.StartsWith("/");
        }
    }
}
