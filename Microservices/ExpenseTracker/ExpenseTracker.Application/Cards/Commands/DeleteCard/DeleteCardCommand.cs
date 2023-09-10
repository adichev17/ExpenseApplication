using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Cards.Commands.DeleteCard
{
    public record DeleteCardCommand (Guid UserId, int CardId) : IRequest<Result<bool>>;
}
