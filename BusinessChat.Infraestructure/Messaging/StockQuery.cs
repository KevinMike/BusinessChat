using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockQuery : IStockQuery
    {
        private readonly IMessageBroker _messageBroker;
        private readonly MessagingConfiguration _messagingConfiguration;
        private readonly ILogger<StockQuery> _logger;

        public StockQuery(IMessageBroker messageBroker, IOptions<MessagingConfiguration> messagingConfiguration, ILogger<StockQuery> logger)
        {
            _logger = logger;
            _messageBroker = messageBroker;
            _messagingConfiguration = messagingConfiguration.Value;

        }

        public void Dispose()
        {
            _messageBroker.Dispose();
        }

        public void Initialize()
        {
            _messageBroker.Initialize(_messagingConfiguration.StockQueryQueueName);
        }

        public Task Publish(StockQueryDTO stockCode)
        {
            return _messageBroker.Publish(stockCode);
        }

        public Task Subscribe(Func<StockQueryDTO, Task> action)
        {
            return _messageBroker.Subscribe(action);
        }
    }
}