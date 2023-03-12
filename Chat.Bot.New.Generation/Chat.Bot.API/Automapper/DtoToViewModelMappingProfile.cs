using AutoMapper;
using Chat.Bot.API.Models;
using Domain.Dtos;

namespace Chat.Bot.API.Automapper
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public DtoToViewModelMappingProfile()
        {
            CreateMap<RoomViewModel, RoomDto>();
            CreateMap<UserViewModel, UserDto>();
            CreateMap<MessageViewModel, MessageDto>();
            CreateMap<ApplicationUser, UserDto>()
                    .ForMember(user => user.UserId, opt => opt.MapFrom(src => src.Id.ToString()))
                    .ForMember(user => user.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(user => user.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                    .ForMember(user => user.Name, opt => opt.MapFrom(src => src.UserName.ToString()));
            CreateMap<ApplicationUser, UserViewModel>()
                    .ForMember(user => user.UserId, opt => opt.MapFrom(src => src.Id.ToString()))
                    .ForMember(user => user.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(user => user.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                    .ForMember(user => user.Name, opt => opt.MapFrom(src => src.UserName.ToString()));

        }
    }
}
