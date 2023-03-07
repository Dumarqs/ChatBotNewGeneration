using Domain.Core.RabbitMQ;
using Infra.CrossCutting.RabbitMQ.Interfaces;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Infra.CrossCutting.RabbitMQ
{
    public class RabbitMQManager : IRabbitMQManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMQManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy);
        }

        public IModel GetChannel()
        {
            var channel = _objectPool.Get();
            return channel;
        }

        public void ReturnChannel(IModel channel)
        {
            _objectPool.Return(channel);
        }
    }
}