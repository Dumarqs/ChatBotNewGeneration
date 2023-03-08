using Application.Services.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Repositories;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<RoomDto> GetAllRoom()
        {
            var room = await _roomRepository.GetAll();
            return _mapper.Map<RoomDto>(room);
        }
    }
}
