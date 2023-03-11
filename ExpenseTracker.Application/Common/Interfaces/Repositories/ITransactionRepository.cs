using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        void AddTransaction(TransactionEntity transaction);
        IQueryable<TransactionEntity> GetAll(int userId, int cardId = 0, int rows = 100);
        Task<TransactionEntity> GetByIdAsync(int transactionId);
    }
}
