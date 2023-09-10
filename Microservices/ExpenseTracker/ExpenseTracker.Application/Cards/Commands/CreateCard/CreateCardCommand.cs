using ExpenseTracker.Application.Common.Dtos.Cards;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.CreateCard
{
    public record CreateCardCommand(
        Guid UserId,
        string CardName,
        int ColorId) : IRequest<Result<CardDto>>;
}
