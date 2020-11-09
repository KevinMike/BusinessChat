using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using Microsoft.AspNetCore.SignalR;

namespace BusinessChat.Webapp.Services
{
    public class NotificationService : INotification
    {
        private  IHubContext<ChatHub> _chatHubContext;
        public NotificationService(IHubContext<ChatHub> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }
        public async Task Notify(object message)
        {
            try
            {
                var stockResponse = (StockResponseDTO)message;
                if (stockResponse.IsSuccesfull)
                {
                    await _chatHubContext.Clients.All.SendAsync("ReceiveOne", "Bot", $"{stockResponse.Stock.Symbol} quote is ${stockResponse.Stock.High} per share");
                }
                else
                {
                    await _chatHubContext.Clients.All.SendAsync("ReceiveOne", "Bot", stockResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await _chatHubContext.Clients.All.SendAsync("ReceiveOne", "Bot", ex.Message);

            }
            
        }
    }
}
