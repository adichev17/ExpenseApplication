using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        void AddTransaction(TransactionEntity transaction);
    }
}
