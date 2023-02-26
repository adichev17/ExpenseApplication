﻿using ExpenseTracker.API.Models.Communications.Expense;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Expenses.Commands.CreateExpense;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    public class ExpenseController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public ExpenseController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            var command = _mapper.Map<CreateTransactionCommand>(request);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok();
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }
        private IActionResult ValidateControlError(IError error)
        {
            return error switch
            {
                CardNotFoundError => Problem(error, HttpStatusCode.NotFound),
                CategoryNotFoundError => Problem(error, HttpStatusCode.NotFound),
                NotFoundUserCategoryError => Problem(error, HttpStatusCode.Conflict),
                _ => Problem(),
            };
        }
    }
}
