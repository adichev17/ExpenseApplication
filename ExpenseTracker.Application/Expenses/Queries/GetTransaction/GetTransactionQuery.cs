using ExpenseTracker.Application.Common.Dtos.Expenses;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries.GetTransaction
{
    public record GetTransactionQuery(
        int TransactionId) : IRequest<Result<TransactionDto>>;
}
