﻿using ExpenseTracker.API.Models.Communications.Card;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using ExpenseTracker.Application.Cards.Commands.DeleteCard;
using ExpenseTracker.Application.Cards.Commands.EditCard;
using ExpenseTracker.Application.Cards.Queries.GetCard;
using ExpenseTracker.Application.Cards.Queries.ListCards;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CardController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        public CardController(IMapper mapper, IMediator mediator, IUserProvider userProvider)
        {
            _mapper= mapper;
            _mediator= mediator;
            _userProvider = userProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
        {
            request.UserId = _userProvider.GetUserId();
            var command = _mapper.Map<CreateCardCommand>(request);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok(commandResult.Value);
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditCardRequest request)
        {
            request.UserId = _userProvider.GetUserId();
            var command = _mapper.Map<EditCardCommand>(request);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok(commandResult.Value);
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int CardId)
        {
            var userId = _userProvider.GetUserId();
            var command = new DeleteCardCommand(userId, CardId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsSuccess)
            {
                return Ok();
            }

            var error = commandResult.Errors.FirstOrDefault();

            return ValidateControlError(error);
        }

        [HttpGet]
        [Route("{userId:int}")]
        public async Task<IActionResult> GetCards()
        {
            var userId = _userProvider.GetUserId();

            var query = new ListCardsQuery(userId);
            var queryResult = await _mediator.Send(query);
            if (queryResult.IsSuccess)
            {
                return Ok(queryResult.Value);
            }
            
            var error = queryResult.Errors.FirstOrDefault();
            return ValidateControlError(error);
        }

        [HttpGet]
        public async Task<IActionResult> GetCard(int cardId)
        {
            var userId = _userProvider.GetUserId();

            var query = new GetCardQuery(userId, cardId);
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
                ColorNotFoundError => Problem(error, HttpStatusCode.NotFound),
                DuplicateCardError => Problem(error, HttpStatusCode.Conflict),
                CardNotFoundError => Problem(error, HttpStatusCode.NotFound),
                _ => Problem(),
            };
        }
    }
}