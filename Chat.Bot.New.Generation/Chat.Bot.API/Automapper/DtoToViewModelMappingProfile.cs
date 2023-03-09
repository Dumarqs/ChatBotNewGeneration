using AutoMapper;
using Chat.Bot.API.ViewModels;
using Domain.Dtos;

namespace Chat.Bot.API.Automapper
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public DtoToViewModelMappingProfile()
        {
            CreateMap<RoomViewModel, RoomDto>();
            CreateMap<UserViewModel, UserDto>();
        }
    }
}
