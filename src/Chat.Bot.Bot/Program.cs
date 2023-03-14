using Application.Services;
using Application.Services.Interfaces;
using Chat.Bot.Bot.Models;
using Chat.Bot.Bot.Services;
using Chat.Bot.Bot.Services.Interfaces;
using Domain.Core.RabbitMQ;
using Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chat.Bot.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })

                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddHostedService<Worker>();

                    services.ServicesLog();
                    services.ServicesRabbitQueue();

                    WorkerParameters options = configuration.GetSection("WorkerParameters").Get<WorkerParameters>();
                    services.TryAddSingleton(options);

                    //RabbitMQ Configs
                    ConnectionMqOptions factory = configuration.GetSection("RabbitConnection").Get<ConnectionMqOptions>();
                    services.TryAddSingleton(factory);

                    services.TryAddSingleton<IStockQuoteService, StockQuoteService>();
                    services.TryAddTransient<IAuthenticateBot, AuthenticateBot>();
                    services.TryAddTransient<ICommandValidation, CommandValidation>();

                    services.AddHttpClient();
                })
                .Build();

            host.Run();
        }
    }
}