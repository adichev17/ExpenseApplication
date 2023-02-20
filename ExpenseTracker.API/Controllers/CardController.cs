using ExpenseTracker.API.Models.Communications.Card;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    public class CardController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CardController(IMapper mapper, IMediator mediator)
        {
            _mapper= mapper;
            _mediator= mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
        {
            var command = _mapper.Map<CreateCardCommand>(request);
            var createResult = await _mediator.Send(command);
            return Ok();
        }
    }
}
