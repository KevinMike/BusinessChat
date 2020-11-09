using System;
using System.Text;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace BusinessChat.Infrastructure.Messaging
{
    public class RabbitMqMessageBroker : IDisposable
    {
        private readonly ILogger<RabbitMqMessageBroker> _logger;
        protected RabbitMqConfiguration _options;
        protected ConnectionFactory _connectionFactory;
        protected IConnection _connection;
        protected IModel _channel;
        protected string _queueName;

        public RabbitMqMessageBroker(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageBroker> logger, string queueName)
        {
            _logger = logger;
            _options = options.Value;
            _queueName = queueName;
            _connectionFactory = new ConnectionFactory
            {
                HostName = _options.Hostname,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                DispatchConsumersAsync = true,
                VirtualHost = "/"
            };
        }


        public void Publish<T>(T message, string exchange)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: exchange, routingKey: _queueName, basicProperties: null, body: body);

                _logger.LogInformation($"Sending {json}  to [{_queueName}]");

            }
        }

        public void Subscribe<T>(Func<T,Task> action)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body);
                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<T>(content);

                _logger.LogInformation($"Message {content} recieved from [{_queueName}]");

                await action(updateCustomerFullNameModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName,false, consumer);
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
            }
            if (_connection != null) {
                _connection.Close();
            }
            _logger.LogInformation("RabbitMQ connection is closed.");
        }

    }
}
