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
    public class RabbitMqMessageBroker : IMessageBroker
    {
        private readonly ILogger<RabbitMqMessageBroker> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private RabbitMqConfiguration _options;
        private string _queueName;

        public RabbitMqMessageBroker(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageBroker> logger)
        {
            _logger = logger;
            _options = options.Value;
            _connectionFactory = new ConnectionFactory
            {
                HostName = _options.Hostname,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                //DispatchConsumersAsync = true
                VirtualHost = "/"
            };
        }

        public void Initialize(string queueName)
        {
            
            _queueName = queueName;
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _logger.LogInformation($"Queue [{_queueName}] is waiting for messages.");
        }

        public Task Publish<T>(T message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                _logger.LogInformation($"Sending {json}  to [{_queueName}]");

            }

            return Task.CompletedTask;
        }

        public Task Subscribe<T>(Func<T,Task> action)
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

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_channel != null) { _channel.Close(); }
            if (_connection != null)
            {
                _connection.Close();
            }
            _logger.LogInformation("RabbitMQ connection is closed.");
        }

    }
}
