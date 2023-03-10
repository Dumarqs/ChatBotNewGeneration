﻿using Application.Services.Interfaces;
using AutoMapper;
using Domain.Chats;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Domain.Repositories;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUserFiltered(Filter filter)
        {
            var user = await _userRepository.GetFiltered(filter);
            return _mapper.Map<IEnumerable<UserDto>>(user);
        }

        public async Task AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();

            await _userRepository.Add(user);
            await _userRepository.SaveChanges();
        }

        public async Task<UserDto> Authenticate(UserDto userDto)
        {            
            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = await _userRepository.Authenticate(userDto.Email);

            // check if username exists
            if (user == null)
                return null;

            if (user.Password == userDto.Password)
                return _mapper.Map<UserDto>(user); 

            return null;
        }
    }
}