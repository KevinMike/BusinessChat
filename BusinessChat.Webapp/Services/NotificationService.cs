using System;
using BusinessChat.Application.Common.Interfaces;

namespace BusinessChat.Webapp.Services
{
    public class NotificationService : INotification
    {
        public void Notify(object message)
        {
            Console.WriteLine("Wujuuuuu");
        }
    }
}
