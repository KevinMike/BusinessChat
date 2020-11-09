using System;
using System.Threading.Tasks;
using BusinessChat.Application.Chat.Commands.SendMessage.Requests;
using BusinessChat.Application.Chat.Queries.GetLastMessages.Requests;
using BusinessChat.Webapp.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BusinessChat.Webapp.Controllers
{
    [ApiController]
    public class ChatController : ApiController
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext, IMediator mediator) : base(mediator)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] int numberOfLastMessages)
        {
            var messages = await _mediator.Send(new GetLastMessagesQueryRequest(numberOfLastMessages));
            return Ok(messages.Messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] MessageDTO msg)
        {
            await _mediator.Send(new SendMessageCommandRequest(msg.Username,msg.Message));
            await _hubContext.Clients.All.SendAsync("ReceiveOne", msg.Username, msg.Message);
            return Ok();
        }
    }
}
