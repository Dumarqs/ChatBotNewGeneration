using Chat.Bot.Bot.Models;
using Chat.Bot.Bot.Services.Interfaces;
using Infra.CrossCutting.Http;
using Infra.CrossCutting.Log.Interfaces;
using System.Text.Json;

namespace Chat.Bot.Bot.Services
{
    public class AuthenticateBot : HttpCore<AuthenticateBot>, IAuthenticateBot
    {
        private readonly ILoggerAdapter<AuthenticateBot> _loggerAdapter;
        private readonly WorkerParameters _options;

        public AuthenticateBot(ILoggerAdapter<AuthenticateBot> logger, IHttpClientFactory httpClient,
                               WorkerParameters options ) : base(logger, httpClient)
        {
            _options= options;
            _loggerAdapter = logger;
        }

        public async Task<string> AuthenticateBotAsync()
        {
            var response = await PostAsync(_options.ApiUrl + "/User/LoginBot", "bot", null);

            if (response.Status == System.Net.HttpStatusCode.OK)
                return JsonSerializer.Deserialize<String>(response.ResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return string.Empty;
        }
    }
}
