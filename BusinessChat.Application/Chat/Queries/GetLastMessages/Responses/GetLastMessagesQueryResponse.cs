using System;
using System.Collections.Generic;
using BusinessChat.Domain.Entities;
using MediatR;

namespace BusinessChat.Application.Chat.Queries.GetLastMessages.Responses
{
    public class GetLastMessagesQueryResponse
    {
        public IEnumerable<ChatMessage> Messages { get; set; }
        public GetLastMessagesQueryResponse(IEnumerable<ChatMessage> messages)
        {
            Messages = messages;
        }
    }
}