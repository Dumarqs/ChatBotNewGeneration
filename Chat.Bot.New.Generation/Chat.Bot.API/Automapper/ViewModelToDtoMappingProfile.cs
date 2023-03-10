using AutoMapper;
using Chat.Bot.API.ViewModels;
using Domain.Dtos;

namespace Chat.Bot.API.Automapper
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public ViewModelToDtoMappingProfile()
        {
            CreateMap<RoomDto, RoomViewModel>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<MessageDto, MessageViewModel>();

        }
    }
}
