using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Common.Models;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BusinessChat.Infrastructure.Messaging
{
    public class StockResponse : RabbitMqMessageBroker , IStockResponse
    {
        private MessagingConfiguration _messagingConfiguration;
        public StockResponse(IOptions<RabbitMqConfiguration> rabbitConfiguration, IOptions<MessagingConfiguration> messagingConfiguration, ILogger<RabbitMqMessageBroker> logger) : base(rabbitConfiguration, logger, messagingConfiguration.Value.StockResponseQueueName)
        {
            _messagingConfiguration = messagingConfiguration.Value;
        }

        public void Initialize()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("amq.fanout", ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare(_queueName, true, false, false, null);
            _channel.QueueBind(_queueName, "amq.fanout", _queueName, null);
            _channel.BasicQos(0, 1, false);
        }

        public void Publish(StockResponseDTO stockCode)
        {
            base.Publish(stockCode, "amq.fanout");
        }

        public void Subscribe(Func<StockResponseDTO, Task> action)
        {
            base.Subscribe(action);
        }
    }
}