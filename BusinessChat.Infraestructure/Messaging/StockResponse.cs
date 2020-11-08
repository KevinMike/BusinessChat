using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Common.Models;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockResponse : IStockResponse
    {
        private readonly IMessageBroker _messageBroker;
        private readonly MessagingConfiguration _messagingConfiguration;

        public StockResponse(IMessageBroker messageBroker, IOptions<MessagingConfiguration> messagingConfiguration)
        {
            _messageBroker = messageBroker;
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public Task Publish(StockResponseDTO stock)
        {
            return _messageBroker.Publish(stock, _messagingConfiguration.StockResponseQueueName);
        }

        public Task Subscribe(Action<StockResponseDTO> action)
        {
            return _messageBroker.Subscribe(_messagingConfiguration.StockResponseQueueName, action);
        }
    }
}