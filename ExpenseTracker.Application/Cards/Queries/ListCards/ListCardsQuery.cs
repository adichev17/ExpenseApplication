using ExpenseTracker.Application.Common.Dtos.Cards;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Queries.ListCards
{
    public record ListCardsQuery(
        int UserId) : IRequest<Result<List<CardDto>>>;
}
