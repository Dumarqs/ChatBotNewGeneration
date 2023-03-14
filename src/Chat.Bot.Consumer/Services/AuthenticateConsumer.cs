using Chat.Bot.Consumer.Models;
using Chat.Bot.Consumer.Services.Interfaces;
using Infra.CrossCutting.Http;
using Infra.CrossCutting.Log.Interfaces;
using System.Text.Json;

namespace Chat.Bot.Consumer.Services
{
    internal class AuthenticateConsumer : HttpCore<string>, IAuthenticateConsumer
    {
        private readonly WorkerParameters _options;

        public AuthenticateConsumer(ILoggerAdapter<string> logger, IHttpClientFactory httpClient,
                                    WorkerParameters options) : base(logger, httpClient)
        {
            _options = options;
        }

        public async Task<string> AuthenticateConsumerAsync(string botConsumer)
        {
            var response = await PostAsync(_options.ApiUrl + "/User/LoginBot", "bot", botConsumer);

            if (response.Status == System.Net.HttpStatusCode.OK)
                return JsonSerializer.Deserialize<String>(response.ResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return string.Empty;
        }
    }
}
