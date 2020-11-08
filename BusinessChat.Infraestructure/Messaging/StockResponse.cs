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

        public void Initialize()
        {
            _messageBroker.Initialize(_messagingConfiguration.StockResponseQueueName);
        }

        public void Dispose()
        {
            _messageBroker.Dispose();
        }

        public Task Publish(StockResponseDTO stock)
        {
            return _messageBroker.Publish(stock);
        }

        public Task Subscribe(Func<StockResponseDTO, Task> action)
        {
            return _messageBroker.Subscribe(action);
        }
    }
}