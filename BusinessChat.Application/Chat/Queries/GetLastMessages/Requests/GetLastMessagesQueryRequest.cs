using BusinessChat.Application.Chat.Queries.GetLastMessages.Responses;
using MediatR;

namespace BusinessChat.Application.Chat.Queries.GetLastMessages.Requests
{
    public class GetLastMessagesQueryRequest : IRequest<GetLastMessagesQueryResponse>
    {
        public int NumberOfLastMessage  { get; set; }
        public GetLastMessagesQueryRequest(int numberOfLastMessage)
        {
            NumberOfLastMessage = numberOfLastMessage;
        }
    }
}
