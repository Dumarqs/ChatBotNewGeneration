using Domain.Core.Hub;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Bot.Bot
{
    public class Worker : IChatBotHub, IHostedService
    {
        private readonly ILoggerAdapter<Worker> _logger;
        private HubConnection _connection;

        public Worker(ILoggerAdapter<Worker> logger)
        {
            _logger = logger;

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7203/chat")
                .Build();

            _connection.On<string>("SendMessage", SendMessage);
        }

        public Task SendMessage(string message)
        {
            _logger.LogInformation(message);

            return Task.CompletedTask;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Keep trying to connect
            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);

                    break;
                }
                catch(Exception ex)
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
    }
}