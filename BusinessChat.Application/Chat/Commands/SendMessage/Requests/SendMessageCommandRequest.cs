using System;
using BusinessChat.Application.Chat.Commands.SendMessage.Responses;
using MediatR;

namespace BusinessChat.Application.Chat.Commands.SendMessage.Requests
{
    public class SendMessageCommandRequest : IRequest<SendMessageCommandResponse>
    {
        public string Message { get; }
        public string Username { get; }
        public SendMessageCommandRequest(string username, string message)
        {
            Message = message;
            Username = username;
        }
    }
}
