using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Hubs;
using Chat.Bot.API.ViewModels;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Controllers
{
    public class ChatController : BaseController
    {
        private readonly ILoggerAdapter<ChatController> _loggerAdapter;
        private readonly IHubContext<ChatBotHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        public ChatController(ILoggerAdapter<ChatController> logger, IHubContext<ChatBotHub> hubContext, IMapper mapper,
                            IMessageService messageService)
        {
            _loggerAdapter= logger;
            _hubContext= hubContext;
            _mapper= mapper;
            _messageService= messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastMessages([FromQuery] int messageQty, [FromQuery] string roomId)
        {
            return Response(_mapper.Map<IEnumerable<MessageViewModel>>(await _messageService.GetLastMessages(messageQty, roomId)));
            //_hubContext.Groups.AddToGroupAsync()   
        }
    }
}
