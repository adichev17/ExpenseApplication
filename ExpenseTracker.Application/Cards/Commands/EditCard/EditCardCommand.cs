using ExpenseTracker.Application.Common.Dtos.Cards;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.EditCard
{
    public record EditCardCommand( 
        Guid UserId,
        int CardId,
        int ColorId,
        string CardName) : IRequest<Result<CardDto>>;
}
