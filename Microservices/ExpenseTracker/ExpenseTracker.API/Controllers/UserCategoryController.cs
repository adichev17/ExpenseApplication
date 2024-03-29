﻿using ExpenseTracker.API.Models.Communications.UserCategory;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory;
using ExpenseTracker.Application.UserCategories.Queries.ListUserCategories;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [Route("api/user/category")]
    public class UserCategoryController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        private Guid UserId { get; init; }
        public UserCategoryController(
            IMapper mapper,
            IMediator mediator,
            IUserProvider userProvider)
        {
            _mapper = mapper;
            _mediator = mediator;
            _userProvider = userProvider;
            UserId = _userProvider.GetUserId();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCategoryRequest request)
        {
            var command = new CreateUserCategoryCommand(UserId, request.CategoryName, request.ActionTypeId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok();
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(int actionTypeId)
        {
            var query = new ListUserCategoriesQuery(UserId, actionTypeId);
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
                UserNotFoundError => Problem(error, HttpStatusCode.NotFound),
                ActionTypeNotFoundError => Problem(error, HttpStatusCode.NotFound),
                DuplicateUserCategoryError => Problem(error, HttpStatusCode.Conflict),
                _ => Problem(),
            };
        }
    }
}
