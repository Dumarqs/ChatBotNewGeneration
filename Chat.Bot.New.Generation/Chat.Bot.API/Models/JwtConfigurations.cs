namespace Chat.Bot.API.Models
{
    public class JwtConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public string ExpiresHours { get; set; }
    }
}
