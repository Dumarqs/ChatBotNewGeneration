using AutoMapper;
using Domain.Chats;
using Domain.Dtos;

namespace Application.Automapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<RoomDto, Room>();
            CreateMap<UserDto, User>();
            CreateMap<MessageDto, Message>();
        }
    }
}
