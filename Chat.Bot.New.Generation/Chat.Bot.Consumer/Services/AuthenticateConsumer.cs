using Chat.Bot.Consumer.Models;
using Chat.Bot.Consumer.Services.Interfaces;
using Infra.CrossCutting.Http;
using Infra.CrossCutting.Log.Interfaces;
using System.Text.Json;

namespace Chat.Bot.Consumer.Services
{
    internal class AuthenticateConsumer : HttpCore<AuthenticateConsumer>, IAuthenticateConsumer
    {
        private readonly ILoggerAdapter<AuthenticateConsumer> _loggerAdapter;
        private readonly WorkerParameters _options;

        public AuthenticateConsumer(ILoggerAdapter<AuthenticateConsumer> logger, IHttpClientFactory httpClient,
                               WorkerParameters options) : base(logger, httpClient)
        {
            _options = options;
            _loggerAdapter = logger;
        }

        public async Task<string> AuthenticateConsumerAsync()
        {
            var response = await PostAsync(_options.ApiUrl + "/User/LoginBot", "bot", null);

            if (response.Status == System.Net.HttpStatusCode.OK)
                return JsonSerializer.Deserialize<String>(response.ResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return string.Empty;
        }
    }
}
