using System;
using System.Threading.Tasks;

namespace BusinessChat.Infrastructure.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        void Initialize(string queueName);
        Task Publish<T>(T message);
        Task Subscribe<T>(Func<T, Task> action);
    }
}
