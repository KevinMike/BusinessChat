using System;
using System.Threading.Tasks;

namespace BusinessChat.Infrastructure.Messaging
{
    public interface IMessageBroker
    {
        Task Publish<T>(T message, string queueName);
        Task Subscribe<T>(string queueName, Action<T> action);
    }
}
