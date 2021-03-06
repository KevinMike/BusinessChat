﻿using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Chat.Commands.SendMessage.Requests;
using BusinessChat.Application.Chat.Commands.SendMessage.Responses;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Domain.Entities;


namespace BusinessChat.Application.Chat.Commands.SendMessage.Handlers
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommandRequest, SendMessageCommandResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IStockQuery _stockQuery;
        public SendMessageCommandHandler(IApplicationDbContext applicationDbContext, IStockQuery stockQuery)
        {
            _applicationDbContext = applicationDbContext;
            _stockQuery = stockQuery;
        }
        public async Task<SendMessageCommandResponse> Handle(SendMessageCommandRequest request, CancellationToken cancellationToken)
        {
            if(request.Message.Contains("/stock="))
            {
                var message = request.Message.Split('=')[1];
                _stockQuery.Publish(new StockQueryDTO(message));
            }
            else
            {
                var message = new ChatMessage()
                {
                    Username = request.Username,
                    Message = request.Message,
                    SendAt = DateTime.UtcNow
                };
                _applicationDbContext.ChatMessages.Add(message);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
            return new SendMessageCommandResponse();
        }
    }
}