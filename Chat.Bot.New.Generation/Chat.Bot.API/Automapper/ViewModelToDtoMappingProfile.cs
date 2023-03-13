using AutoMapper;
using Chat.Bot.API.Models;
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
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(user => user.Id, opt => opt.MapFrom(src => src.UserId.ToString()))
                .ForMember(user => user.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(user => user.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(user => user.UserName, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(user => user.NormalizedUserName, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(user => user.Role, opt => opt.MapFrom(src => src.Role));


        }
    }
}
