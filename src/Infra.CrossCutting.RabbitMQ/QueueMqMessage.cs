using Infra.CrossCutting.RabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infra.CrossCutting.RabbitMQ
{
    public class QueueMqMessage
    {
        private IRabbitMQManager _rabbitMQManager;
        private readonly string _exchangeName = string.Empty;
        private readonly string _queueName;
        private readonly int _recordsPerBatch;
        private IModel channel;

        public QueueMqMessage(IRabbitMQManager rabbitMQManager, string exchangeName, string queueName, int recordsPerBatch)
        {
            _rabbitMQManager = rabbitMQManager;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _recordsPerBatch = recordsPerBatch;
            channel = _rabbitMQManager.GetChannel();
        }

        public void QueueMessage<T>(IList<T> obj)
        {
            //Declare a queue
            //channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            decimal totalIteration;
            if (_recordsPerBatch > 0)
                totalIteration = Math.Ceiling((decimal)obj.Count / _recordsPerBatch);
            else
                totalIteration = obj.Count;

            for (int i = 0; i < totalIteration; i++)
            {
                var listResultPage = obj.Skip(i * _recordsPerBatch).Take(_recordsPerBatch).ToList();

                //Serialize the object to JSON
                var content = JsonConvert.SerializeObject(listResultPage);

                byte[] body = Encoding.Default.GetBytes(content);
                IBasicProperties properties = channel.CreateBasicProperties();

                //Set the message as Persistent
                properties.Persistent = true;

                //Send the Message to the QUEUE
                channel.BasicPublish(exchange: _exchangeName.ToString(), routingKey: _queueName, basicProperties: properties, body: body);
            }

            _rabbitMQManager.ReturnChannel(channel);
        }

        public void QueueMessage<T>(T obj)
        {
            //Declare a queue
            //channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            //Serialize the object to JSON
            var content = JsonConvert.SerializeObject(obj);

            byte[] body = Encoding.Default.GetBytes(content);
            IBasicProperties properties = channel.CreateBasicProperties();

            //Set the message as Persistent
            properties.Persistent = true;

            //Send the Message to the QUEUE
            channel.BasicPublish(exchange: _exchangeName.ToString(), routingKey: _queueName, basicProperties: properties, body: body);

            _rabbitMQManager.ReturnChannel(channel);
        }
    }
}
