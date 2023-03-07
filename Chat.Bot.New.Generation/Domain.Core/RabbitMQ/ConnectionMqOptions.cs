namespace Domain.Core.RabbitMQ
{
    /// <summary>
    /// Properties Connection Factory
    /// </summary>
    public class ConnectionMqOptions
    {
        /// <summary>
        /// Set the connection name
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// Uri
        /// </summary>
        public string Uri { get; set; }
    }
}
