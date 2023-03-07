using RabbitMQ.Client;

namespace Infra.CrossCutting.RabbitMQ.Interfaces
{
    /// <summary>
    /// Manage the channels
    /// </summary>
    public interface IRabbitMQManager
    {
        /// <summary>
        /// Get a channel in the pool of Channels
        /// </summary>
        /// <returns>Return a channel</returns>
        IModel GetChannel();

        /// <summary>
        /// Return the Channel to the pool
        /// </summary>
        /// <param name="channel">IModel</param>
        void ReturnChannel(IModel channel);
    }
}
