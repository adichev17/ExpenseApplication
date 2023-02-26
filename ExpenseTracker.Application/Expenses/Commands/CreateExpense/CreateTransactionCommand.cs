using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Commands.CreateExpense
{
    public record CreateTransactionCommand(
        int CardId,
        decimal Amount,
        string Comment,
        int CategoryId) : IRequest<Result<bool>>;
}
