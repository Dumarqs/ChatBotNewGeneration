namespace Chat.Bot.UI.Model
{
    public class Config
    {
        private readonly string baseUrl;

        public Config(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public string GetBaseUrl()
        {
            return baseUrl.ToString();
        }
    }
}
