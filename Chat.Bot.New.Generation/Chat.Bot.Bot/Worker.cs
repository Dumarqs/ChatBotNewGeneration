using Application.Services.Interfaces;
using Chat.Bot.Bot.Models;
using Chat.Bot.Bot.Services;
using Chat.Bot.Bot.Services.Interfaces;
using Chat.Bot.Bot.ViewModels;
using Infra.CrossCutting.Log.Interfaces;
using Infra.CrossCutting.RabbitMQ;
using Infra.CrossCutting.RabbitMQ.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Channels;

namespace Chat.Bot.Bot
{
    public class Worker : IHostedService
    {
        private readonly ILoggerAdapter<Worker> _logger;
        private HubConnection _connection;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IRabbitMQManager _rabbitMQ;
        private readonly WorkerParameters _options;
        private readonly IAuthenticateBot _authenticateBot;
        private readonly ICommandValidation _commandValidation;

        public Worker(ILoggerAdapter<Worker> logger, IStockQuoteService stockQuoteService, IRabbitMQManager rabbitMQ,
                      WorkerParameters options, IAuthenticateBot authenticateBot, ICommandValidation commandValidation)
        {
            _logger = logger;
            _stockQuoteService = stockQuoteService;
            _rabbitMQ = rabbitMQ;
            _options = options;
            _authenticateBot = authenticateBot;
            _commandValidation = commandValidation;
        }

        public async Task MessageReceive(Message message)
        {
            if (message.IsCommand())
            {
                var channel = new QueueMqMessage(_rabbitMQ, _options.ExchangeName, _options.QueueName, _options.RecordsPerBatch);
                var command = _commandValidation.IsValidCommand(message.Text);
                if (!command.IsValid)
                {
                    channel.QueueMessage("Command is not valid!");
                }

                var quote = await _stockQuoteService.GetStockQuoteCSV(_options.UriCsv, command.Value);
                _logger.LogInformation($"Command Received: {quote}");

                channel.QueueMessage(quote);
            }
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            CreateQueue();

            var token = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    token = await _authenticateBot.AuthenticateBotAsync();
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
                throw new Exception("Bot not authenticated");

            CreateHubConnection(token);
            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);

                    break;
                }
                catch (Exception ex)
                {
                    //if fails try again in one second
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.DisposeAsync();
        }

        private void CreateQueue()
        {
            var channel = _rabbitMQ.GetChannel();

            channel.ExchangeDeclare(
                    exchange: _options.ExchangeName,
                    type: "fanout",
                    durable: false,
                    autoDelete: false,
                    arguments: null
                );

            channel.QueueDeclare(
                     queue: _options.QueueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.QueueBind(_options.QueueName, _options.ExchangeName, "", null);
        }

        private void CreateHubConnection(string token)
        {
            _connection = new HubConnectionBuilder()
                        .WithUrl(_options.ApiUrl + "/chat", options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(token);
                        })
                        .Build();

            _connection.On<Message>("Message", MessageReceive);
        }
    }
}