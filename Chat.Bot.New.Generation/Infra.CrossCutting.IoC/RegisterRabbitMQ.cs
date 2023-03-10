using Infra.CrossCutting.RabbitMQ;
using Infra.CrossCutting.RabbitMQ.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Infra.CrossCutting.IoC
{
    public static class RegisterRabbit
    {
        public static void ServicesRabbitQueue(this IServiceCollection services)
        {
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();
        }
    }
}
