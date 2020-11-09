using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockQuery : RabbitMqMessageBroker , IStockQuery
    {
        MessagingConfiguration _messagingConfiguration;
        public StockQuery(IOptions<RabbitMqConfiguration> rabbitConfiguration, IOptions<MessagingConfiguration> messagingConfiguration, ILogger<RabbitMqMessageBroker> logger) : base(rabbitConfiguration, logger, messagingConfiguration.Value.StockQueryQueueName)
        {
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public void Initialize()
        {
            this._connection = _connectionFactory.CreateConnection();
            this._channel = _connection.CreateModel();
            _channel.ExchangeDeclare("amq.direct", ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(_queueName, true, false, false, null);
            _channel.QueueBind(_queueName, "amq.direct", _queueName, null);
            _channel.BasicQos(0, 1, false);
        }

        public void Publish(StockQueryDTO stockCode)
        {
            base.Publish(stockCode, "amq.direct");
        }

        public void Subscribe(Func<StockQueryDTO, Task> action)
        {
            base.Subscribe(action);
        }
    }
}