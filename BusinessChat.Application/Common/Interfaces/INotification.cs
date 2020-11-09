using System;
using System.Threading.Tasks;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface INotification
    {
        Task Notify(object message);
    }
}
