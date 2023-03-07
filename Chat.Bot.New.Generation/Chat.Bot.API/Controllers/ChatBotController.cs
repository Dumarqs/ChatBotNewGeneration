using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Hubs;
using Domain.Core.Hub;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Controllers
{
    public class ChatBotController : BaseController
    {
        private readonly ILoggerAdapter<ChatBotController> _loggerAdapter;
        private readonly IHubContext<ChatBotHub, IChatBotHub> _hubContext;
        public ChatBotController(ILoggerAdapter<ChatBotController> logger, IHubContext<ChatBotHub, IChatBotHub> hubContext)
        {
            _loggerAdapter= logger;
            _hubContext= hubContext;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage()
        {
            _loggerAdapter.LogInformation($"Message Received at: {DateTime.Now}");
            await _hubContext.Clients.All.SendMessage($"Home page loaded at: {DateTime.Now}");

            return Ok();

            //return BadRequest();
        }
    }
}
