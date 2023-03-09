using System.Text.Json.Serialization;

namespace Chat.Bot.UI.Model
{
    public class User
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }
    }
}
