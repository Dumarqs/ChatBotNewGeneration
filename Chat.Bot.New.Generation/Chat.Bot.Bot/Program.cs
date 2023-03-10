using Application.Services;
using Chat.Bot.Bot.Models;
using Infra.CrossCutting.IoC;
using Microsoft.Extensions.Configuration;

namespace Chat.Bot.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddHostedService<Worker>();

                    services.ServicesLog();
                    services.ServicesRabbitQueue();

                    WorkerParameters options = configuration.GetSection("WorkerOptions").Get<WorkerParameters>();
                    services.AddSingleton(options);
                    services.AddScoped<IStockQuoteService, StockQuoteService>();
                })
                .Build();

            host.Run();
        }
    }
}