namespace Chat.Bot.API.Models
{
    public class MessageViewModel
    {
        public Guid MessageId { get; set; }
        public UserViewModel User { get; set; }
        public Guid RoomId { get; set; }
        public string Text { get; set; }
        public DateTime DtInserted { get; set; }

        public bool IsCommand()
        {
            return Text.StartsWith("/");
        }
    }
}
