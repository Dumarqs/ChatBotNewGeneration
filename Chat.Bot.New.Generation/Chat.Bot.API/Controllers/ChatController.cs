using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Hubs;
using Chat.Bot.API.Models;
using Domain.Dtos;
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
        private readonly IUserService _userService;
        public ChatController(ILoggerAdapter<ChatController> logger, IHubContext<ChatBotHub> hubContext, IMapper mapper,
                            IMessageService messageService, IUserService userService)
        {
            _loggerAdapter = logger;
            _hubContext = hubContext;
            _mapper = mapper;
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastMessages([FromQuery] int messageQty, [FromQuery] string roomId)
        {
            return Response(_mapper.Map<IEnumerable<MessageViewModel>>(await _messageService.GetLastMessages(messageQty, roomId)));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageClient([FromBody] MessageDto message)
        {
            var user = await _userService.GetUser(message.User.UserId);
            await _hubContext.Clients.Client(user.ConnectionId).SendAsync("Message", message);

            return Ok();
        }
    }
}
