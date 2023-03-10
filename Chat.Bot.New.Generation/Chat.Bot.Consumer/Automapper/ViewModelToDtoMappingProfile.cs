using AutoMapper;
using Chat.Bot.Consumer.Models;
using Domain.Dtos;

namespace Chat.Bot.Consumer.Automapper
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public ViewModelToDtoMappingProfile()
        {
            CreateMap<MessageDto, QuoteMessage>();
        }
    }
}
