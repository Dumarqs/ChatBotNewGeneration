using Application.Services.Interfaces;
using Chat.Bot.API.Controllers.Base;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers
{
    public class RoomController : BaseController
    {
        private readonly ILoggerAdapter<RoomController> _logger;
        private readonly IRoomService _roomService;
        public RoomController(ILoggerAdapter<RoomController> logger, 
                              IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoom()
        {
            return Response(await _roomService.GetAllRoom());
        }
    }
}
