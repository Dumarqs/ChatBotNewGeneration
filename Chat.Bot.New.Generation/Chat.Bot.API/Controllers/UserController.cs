using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.ViewModels;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILoggerAdapter<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<UserViewModel> _userManager;

        public UserController(ILoggerAdapter<UserController> logger,
                              IUserService userService, IMapper mapper,
                              UserManager<UserViewModel> userManager)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFiltered([FromQuery] Filter filter)
        {
            return Response(_mapper.Map<IEnumerable<UserViewModel>>(await _userService.GetUserFiltered(filter)));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel userViewModel)
        {
            var userDto = _mapper.Map<UserDto>(userViewModel);
            await _userService.AddUser(userDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserViewModel userViewModel)
        {

            var userName = await _userManager.FindByNameAsync(userViewModel.Email);

            var userDto = _mapper.Map<UserDto>(userViewModel);
            await _userService.AddUser(userDto);
            return Ok();
        }
    }
}
