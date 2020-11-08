using System;
using System.Text;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace BusinessChat.Infrastructure.Messaging
{
    public class RabbitMqMessageBroker : IMessageBroker
    {

        protected readonly RabbitMqConfiguration _connectionSettings;
        protected readonly ConnectionFactory _connectionFactory;
        protected IConnection _connection;
        protected IModel _channel;

        public RabbitMqMessageBroker(IOptions<RabbitMqConfiguration> connectionSettings)
        {
            this._connectionSettings = connectionSettings.Value;
            this._connectionFactory = new ConnectionFactory
            {
                UserName = this._connectionSettings.UserName,
                Password = this._connectionSettings.Password,
                HostName = this._connectionSettings.Hostname,
                Port = _connectionSettings.Port
            };
        }

        public Task Publish<T>(T message, string queueName)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string mg = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(mg);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

            return Task.CompletedTask;
        }

        public Task Subscribe<T>(string queueName,Action<T> action)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received +=  (model, ea) =>
                {
                    //var body = ea.Body.ToArray();
                    //var message = Encoding.UTF8.GetString(body);
                    var content = Encoding.UTF8.GetString(ea.Body);
                    var payload = JsonSerializer.Deserialize<T>(content);
                    action(payload);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (this._channel != null)
            {
                this._connection.Close();
                this._channel.Close();
            }
        }
    }
}
