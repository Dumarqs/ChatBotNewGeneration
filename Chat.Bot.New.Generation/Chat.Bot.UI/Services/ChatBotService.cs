namespace Chat.Bot.UI.Services
{
    public class ChatBotService
    {
        private readonly HttpClient httpClient;

        public ChatBotService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public string GetBaseUrl()
        {
            return httpClient.BaseAddress.ToString();
        }
    }
}
