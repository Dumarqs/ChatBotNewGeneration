using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Models;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers
{
    public class RoomController : BaseController
    {
        private readonly ILoggerAdapter<RoomController> _logger;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        public RoomController(ILoggerAdapter<RoomController> logger,
                              IRoomService roomService, IMapper mapper)
        {
            _logger = logger;
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoom()
        {
            return Response(_mapper.Map<IEnumerable<RoomViewModel>>(await _roomService.GetAllRoom()));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomFiltered([FromQuery] Filter filter)
        {
            return Response(_mapper.Map<IEnumerable<RoomViewModel>>(await _roomService.GetRoomFiltered(filter)));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom([FromBody] RoomViewModel roomViewModel)
        {
            var roomDto = _mapper.Map<RoomDto>(roomViewModel);
            var room = await _roomService.AddRoom(roomDto);
            return Ok(_mapper.Map<RoomViewModel>(room));
        }
    }
}
