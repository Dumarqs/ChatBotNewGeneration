using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.Consumer.Models;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Infra.CrossCutting.RabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Chat.Bot.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILoggerAdapter<Worker> _logger;
        private readonly WorkerParameters _options;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly IBotService _botService;
        private readonly IMapper _mapper;

        public Worker(ILoggerAdapter<Worker> logger, WorkerParameters options,
                    IRabbitMQManager rabbitMQManager, IBotService botService,
                    IMapper mapper)
        {
            _logger = logger;
            _options = options;
            _rabbitMQManager = rabbitMQManager;
            _botService = botService;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = _rabbitMQManager.GetChannel();
            await ConsumerQueue(channel);
        }

        private async Task ConsumerQueue(IModel channel)
        {
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(channel);
            
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var content = Encoding.UTF8.GetString(body.Span);
                    var message = JsonConvert.DeserializeObject<List<QuoteMessage>>(content);

                    await _botService.SendMessageAsync(_mapper.Map<MessageDto>(message), _options.ApiUrl);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{ex.Message} \n {ex.InnerException}");
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: true, requeue: false);
                }                
            };

            channel.BasicConsume(queue: _options.QueueName, autoAck: false, consumer: consumer);
        }
    }
}