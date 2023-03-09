using AutoMapper;
using Domain.Chats;
using Domain.Dtos;

namespace Application.Automapper
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<User, UserDto>();

        }
    }
}
