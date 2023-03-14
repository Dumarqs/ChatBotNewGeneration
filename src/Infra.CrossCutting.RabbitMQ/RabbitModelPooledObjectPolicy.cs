using Domain.Core.RabbitMQ;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Infra.CrossCutting.RabbitMQ
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly ConnectionMqOptions _cnnFactory;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(ConnectionMqOptions cnnFactory)
        {
            _cnnFactory = cnnFactory;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_cnnFactory.Uri),
                    DispatchConsumersAsync = true
                };

                return factory.CreateConnection(_cnnFactory.ConnectionName);
            }
            catch (Exception exRabbitMQ)
            {
                throw new Exception($"Problem to create a connection with RabbitMQ.{exRabbitMQ.InnerException}|{exRabbitMQ.Message}");
            }
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
