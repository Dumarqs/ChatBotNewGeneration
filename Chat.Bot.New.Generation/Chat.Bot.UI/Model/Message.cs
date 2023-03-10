using System.Text.Json.Serialization;

namespace Chat.Bot.UI.Model
{
    public class Message
    {
        [JsonPropertyName("messageId")]
        public Guid MessageId { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("roomId")]
        public Guid RoomId { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("dtInserted")]
        public DateTime DtInserted { get; set; }
    }
}
