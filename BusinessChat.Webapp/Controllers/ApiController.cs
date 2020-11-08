using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BusinessChat.Webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly IMediator _mediator;
        public ApiController(IMediator mediator) : base()
        {
            _mediator = mediator;
        }
    }
}
