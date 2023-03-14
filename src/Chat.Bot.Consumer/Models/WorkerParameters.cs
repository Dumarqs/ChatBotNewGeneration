namespace Chat.Bot.Consumer.Models
{
    /// <summary>
    /// Worker option Properties
    /// </summary>
    public class WorkerParameters
    {
        public string QueueName { get; set; }
        public string ApiUrl { get; set; }
        public string UserConsumer { get; set; }
    }
}
