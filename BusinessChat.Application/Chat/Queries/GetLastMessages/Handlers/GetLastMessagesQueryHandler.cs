using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Chat.Queries.GetLastMessages.Requests;
using BusinessChat.Application.Chat.Queries.GetLastMessages.Responses;
using BusinessChat.Application.Common.Interfaces;
using MediatR;

namespace BusinessChat.Application.Chat.Queries.GetLastMessages.Handlers
{
    public class GetLastMessagesQueryHandler : IRequestHandler<GetLastMessagesQueryRequest, GetLastMessagesQueryResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public GetLastMessagesQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<GetLastMessagesQueryResponse> Handle(GetLastMessagesQueryRequest request, CancellationToken cancellationToken)
        {
            var messages = _applicationDbContext.ChatMessages
                                .OrderByDescending(p => p.Created)
                                .Take(request.NumberOfLastMessage)
                                .OrderBy(p=>p.Created)
                                .ToList();
            return Task.FromResult(new GetLastMessagesQueryResponse(messages));
        }
    }
}
