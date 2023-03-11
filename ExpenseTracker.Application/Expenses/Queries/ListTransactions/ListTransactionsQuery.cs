using ExpenseTracker.Application.Common.Dtos.Expenses;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Expenses.Queries.ListTransactions
{
    public record ListTransactionsQuery(
        int UserId, int Rows, int CardId = 0) : IRequest<Result<List<CardTransactionsDto>>>;
}
