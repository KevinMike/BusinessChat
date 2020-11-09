using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockQuery : RabbitMqMessageBroker , IStockQuery
    {
        MessagingConfiguration _messagingConfiguration;
        public StockQuery(IOptions<RabbitMqConfiguration> rabbitConfiguration, IOptions<MessagingConfiguration> messagingConfiguration, ILogger<RabbitMqMessageBroker> logger) : base(rabbitConfiguration, logger, messagingConfiguration.Value.StockQueryQueueName)
        {
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public void Publish(StockQueryDTO stockCode)
        {
            base.Publish(stockCode);
        }

        public void Subscribe(Func<StockQueryDTO, Task> action)
        {
            base.Subscribe(action);
        }
    }
}