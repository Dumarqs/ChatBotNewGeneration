using Infra.CrossCutting.IoC;

namespace Chat.Bot.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();

                    services.ServicesLog();
                })
                .Build();

            host.Run();
        }
    }
}