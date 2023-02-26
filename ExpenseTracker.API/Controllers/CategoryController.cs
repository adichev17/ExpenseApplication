using ExpenseTracker.API.Models.Communications.Category;
using ExpenseTracker.Application.Categories.Commands;
using ExpenseTracker.Application.Common.Errors.Controls;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/admin/[controller]")]
    public class CategoryController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CategoryController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var command = _mapper.Map<CreateCategoryCommand>(request);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok(commandResult.Value);
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        private IActionResult ValidateControlError(IError error)
        {
            return error switch
            {
                DuplicateCategoryError => Problem(error, HttpStatusCode.Conflict),
                _ => Problem(),
            };
        }
    }
}
