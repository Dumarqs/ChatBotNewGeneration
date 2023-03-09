using Application.Services.Interfaces;
using AutoMapper;
using Domain.Chats;
using Domain.Core.SqlServer;
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

        public async Task<IEnumerable<RoomDto>> GetAllRoom()
        {
            var room = await _roomRepository.GetAll();
            return _mapper.Map<IEnumerable<RoomDto>>(room);
        }

        public async Task<IEnumerable<RoomDto>> GetRoomFiltered(Filter filter)
        {
            var room = await _roomRepository.GetFiltered(filter);
            return _mapper.Map<IEnumerable<RoomDto>>(room);
        }

        public async Task AddRoom(RoomDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);
            room.RoomId = Guid.NewGuid();

            await _roomRepository.Add(room);
            await _roomRepository.SaveChanges();
        }
    }
}
