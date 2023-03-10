namespace Chat.Bot.Bot.Models
{
    /// <summary>
    /// Worker option Properties
    /// </summary>
    public class WorkerParameters
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public int RecordsPerBatch { get; set; }
        public string UriCSV { get; set; }
    }
}
