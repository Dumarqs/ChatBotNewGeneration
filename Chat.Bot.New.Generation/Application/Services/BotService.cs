using Application.Services.Interfaces;
using Domain.Dtos;
using Infra.CrossCutting.Http;
using Infra.CrossCutting.Log.Interfaces;

namespace Application.Services
{
    public class BotService : HttpCore<MessageDto>, IBotService
    {
        private readonly ILoggerAdapter<MessageDto> _logger;

        public BotService(ILoggerAdapter<MessageDto> logger, IHttpClientFactory httpClientFactory) :
            base(logger, httpClientFactory)
        {
            _logger = logger;
        }

        public async Task SendMessageAsync(MessageDto message, string uri)
        {
            await PostAsync(uri + "/chat/sendMessageClient", "bot", message);
        }
    }
}
