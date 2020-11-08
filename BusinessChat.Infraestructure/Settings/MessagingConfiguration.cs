using System;
namespace BusinessChat.Infrastructure.Settings
{
    public class MessagingConfiguration
    {
        public string StockQueryQueueName { get; set; }
        public string StockResponseQueueName { get; set; }
    }
}
