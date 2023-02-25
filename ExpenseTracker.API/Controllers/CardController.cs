using ExpenseTracker.API.Models.Communications.Card;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using ExpenseTracker.Application.Common.Errors.Controls;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]/[action]")]
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
            var commadResult = await _mediator.Send(command);
            if (commadResult.IsSuccess)
            {
                return Ok(commadResult.Value);
            }

            var error = commadResult.Errors.FirstOrDefault();

            switch (error)
            {
                case UserNotFoundError:
                    return Problem(error, HttpStatusCode.BadRequest);
                case ColorNotFoundError:
                    return Problem(error, HttpStatusCode.BadRequest);
                case DuplicateCardError:
                    return Problem(error, HttpStatusCode.Conflict);
                default:
                    break;
            }

            return Problem();
        }
    }
}
