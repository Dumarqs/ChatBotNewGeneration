using Application.Services.Interfaces;
using AutoMapper;
using Domain.Chats;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Domain.Repositories;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDto>> GetLastMessages(int messageQty, string roomId)
        {
            var filter = new Filter { Field = "RoomId = @0", Search = roomId, Take = messageQty };
            var messages = await _messageRepository.GetMessagesFiltered(filter);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task SaveMessage(MessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            message.MessageId = Guid.NewGuid();

            await _messageRepository.AddMessage(message);
        }
    }
}
