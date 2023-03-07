using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Hubs;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Controllers
{
    public class ChatBotController : BaseController
    {
        private readonly ILoggerAdapter<ChatBotController> _loggerAdapter;
        private readonly IHubContext<ChatBotHub> _hubContext;
        public ChatBotController(ILoggerAdapter<ChatBotController> logger, IHubContext<ChatBotHub> hubContext)
        {
            _loggerAdapter= logger;
            _hubContext= hubContext;
        }
    }
}
