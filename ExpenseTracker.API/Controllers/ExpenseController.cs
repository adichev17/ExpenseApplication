using ExpenseTracker.API.Models.Communications.Expense;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Expenses.Commands.CreateExpense;
using ExpenseTracker.Application.Expenses.Queries.GetTransaction;
using ExpenseTracker.Application.Expenses.Queries.ListTransactions;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ExpenseController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        public ExpenseController(
            IMapper mapper, 
            IMediator mediator,
            IUserProvider userProvider)
        {
            _mapper = mapper;
            _mediator = mediator;
            _userProvider = userProvider;
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

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions(int cardId = 0, int rows = 100)
        {
            var userId = _userProvider.GetUserId();
            var query = new ListTransactionsQuery(userId, rows, cardId);
            var queryResult = await _mediator.Send(query);
            if (queryResult.IsSuccess)
            {
                return Ok(queryResult.Value);
            }

            var error = queryResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        [HttpGet("{transactionId:int}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var query = new GetTransactionQuery(transactionId);
            var queryResult = await _mediator.Send(query);
            if (queryResult.IsSuccess)
            {
                return Ok(queryResult.Value);
            }

            var error = queryResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        private IActionResult ValidateControlError(IError error)
        {
            return error switch
            {
                CardNotFoundError => Problem(error, HttpStatusCode.NotFound),
                CategoryNotFoundError => Problem(error, HttpStatusCode.NotFound),
                UserCategoryNotFoundError => Problem(error, HttpStatusCode.Conflict),
                NotFoundTransactionError => Problem(error, HttpStatusCode.NotFound),
                _ => Problem(),
            };
        }
    }
}
