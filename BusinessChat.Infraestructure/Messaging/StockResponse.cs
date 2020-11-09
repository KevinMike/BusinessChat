using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Common.Models;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockResponse : RabbitMqMessageBroker , IStockResponse
    {
        private MessagingConfiguration _messagingConfiguration;
        public StockResponse(IOptions<RabbitMqConfiguration> rabbitConfiguration, IOptions<MessagingConfiguration> messagingConfiguration, ILogger<RabbitMqMessageBroker> logger) : base(rabbitConfiguration, logger, messagingConfiguration.Value.StockResponseQueueName)
        {
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public void Publish(StockResponseDTO stockCode)
        {
            base.Publish(stockCode);
        }

        public void Subscribe(Func<StockResponseDTO, Task> action)
        {
            base.Subscribe(action);
        }
    }
}