using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Bot.Consumer.Models
{
    /// <summary>
    /// Worker option Properties
    /// </summary>
    public class WorkerParameters
    {
        public string QueueName { get; set; }
        public string ApiUrl { get; set; }
    }
}
