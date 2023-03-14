using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.Consumer.Models;
using Chat.Bot.Consumer.Services.Interfaces;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Infra.CrossCutting.RabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Chat.Bot.Consumer
{
    public class Worker : IHostedService
    {
        private readonly ILoggerAdapter<Worker> _logger;
        private readonly WorkerParameters _options;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly IBotService _botService;
        private readonly IMapper _mapper;
        private readonly IAuthenticateConsumer _authenticateConsumer;
        private string token;

        public Worker(ILoggerAdapter<Worker> logger, WorkerParameters options,
                    IRabbitMQManager rabbitMQManager, IBotService botService,
                    IMapper mapper, IAuthenticateConsumer authenticateConsumer)
        {
            _logger = logger;
            _options = options;
            _rabbitMQManager = rabbitMQManager;
            _botService = botService;
            _mapper = mapper;
            _authenticateConsumer = authenticateConsumer;
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
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
                    var message = JsonConvert.DeserializeObject<MessageDto>(content);

                    await _botService.SendMessageAsync(message, _options.ApiUrl, token);

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    token = await _authenticateConsumer.AuthenticateConsumerAsync(_options.UserConsumer);
                    break;
                }
                catch (Exception ex)
                {
                    //if fails try again in one second
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(1000, cancellationToken);
                }
            }

            if (string.IsNullOrEmpty(token))
                throw new Exception("Consumer not authenticated");

            await ExecuteAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}