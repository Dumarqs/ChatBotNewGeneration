using Application.Services;
using Application.Services.Interfaces;
using Chat.Bot.Bot.Models;
using Domain.Core.RabbitMQ;
using Infra.CrossCutting.IoC;
using Microsoft.Extensions.Configuration;

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
                    services.AddSingleton(options);

                    //RabbitMQ Configs
                    ConnectionMqOptions factory = configuration.GetSection("RabbitConnection").Get<ConnectionMqOptions>();
                    services.AddSingleton(factory);

                    services.AddSingleton<IStockQuoteService, StockQuoteService>();

                    services.AddHttpClient();
                })
                .Build();

            host.Run();
        }
    }
}