﻿using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Controllers.Base;
using Chat.Bot.API.Models;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Bot.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILoggerAdapter<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtConfigurations _jwtConfigurations;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ILoggerAdapter<UserController> logger,
                              IUserService userService, 
                              IMapper mapper,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager, 
                              JwtConfigurations jwtConfigurations, 
                              RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfigurations = jwtConfigurations;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFiltered([FromQuery] Filter filter)
        {
            return Response(_mapper.Map<IEnumerable<UserViewModel>>(await _userService.GetUserFiltered(filter)));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User logged in.");

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                };

                    var securityToken = GetToken(authClaims);
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.WriteToken(securityToken);

                    return Ok(token);
                }
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginBot()
        {
            var user = new ApplicationUser
            {
                Email = "bot@test.com",
                UserName = "bot@test.com",
                PasswordHash = "solo322"
            };

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("Bot logged.");

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Bot")
                };

            var securityToken = GetToken(authClaims);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(securityToken);

            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Email= model.Email,
                UserName= model.Name,
                PasswordHash= model.Password
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created a new account with password.");
                return Ok();
            }

            return BadRequest();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtConfigurations.Issuer,
                //audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(_jwtConfigurations.ExpiresHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
