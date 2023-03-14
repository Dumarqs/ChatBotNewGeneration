using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Chat.Bot.UI.Model
{
    public class Room
    {
        [JsonPropertyName("roomId")]
        public Guid RoomId { get; set; }
        [MinLength(5)]
        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }
    }
}
