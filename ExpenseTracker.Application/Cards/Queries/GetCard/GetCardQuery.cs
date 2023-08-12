using ExpenseTracker.Application.Common.Dtos.Cards;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Queries.GetCard
{
    public record GetCardQuery(
        Guid UserId,
        int CardId) : IRequest<Result<CardDto>>;
}
