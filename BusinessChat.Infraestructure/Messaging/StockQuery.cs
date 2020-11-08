using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockQuery : IStockQuery
    {
        private readonly IMessageBroker _messageBroker;
        private readonly MessagingConfiguration _messagingConfiguration;
        public StockQuery(IMessageBroker messageBroker, IOptions<MessagingConfiguration> messagingConfiguration)
        {
            _messageBroker = messageBroker;
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public Task Publish(StockQueryDTO stockSymbol)
        {
            return _messageBroker.Publish(stockSymbol, _messagingConfiguration.StockQueryQueueName);
        }

        public Task Subscribe(Action<StockQueryDTO> action)
        {
            return _messageBroker.Subscribe(_messagingConfiguration.StockQueryQueueName, action);
        }
    }
}