using AutoMapper;
using Chat.Bot.Consumer.Models;
using Domain.Dtos;

namespace Chat.Bot.Consumer.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<QuoteMessage, MessageDto>().ReverseMap()
                    .ForMember(message => message.MessageId, opt => opt.MapFrom(src => src.MessageId.ToString()))
                    .ForMember(message => message.User, opt => opt.MapFrom(src => src.User))
                    .ForMember(message => message.RoomId, opt => opt.MapFrom(src => src.RoomId))
                    .ForMember(message => message.Text, opt => opt.MapFrom(src => src.Text))
                    .ForMember(message => message.DtInserted, opt => opt.MapFrom(src => src.DtInserted));
        }
    }
}
