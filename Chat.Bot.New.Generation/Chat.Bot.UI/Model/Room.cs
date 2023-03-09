using System.Text.Json.Serialization;

namespace Chat.Bot.UI.Model
{
    public class Room
    {
        [JsonPropertyName("roomId")]
        public Guid RoomId { get; set; }
        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }
    }
}
