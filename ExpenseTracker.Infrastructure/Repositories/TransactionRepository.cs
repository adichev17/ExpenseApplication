using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Persistence;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ExpenseTrackerDBContext _context;
        public TransactionRepository(ExpenseTrackerDBContext context)
        {
            _context = context;
        }

        public void AddTransaction(TransactionEntity transactionEntity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var card = _context.Cards.FirstOrDefault(x => x.Id == transactionEntity.CardId);
                    var category = _context.Categories.FirstOrDefault(x => x.Id == transactionEntity.CategoryId);

                    _context.Transactions.Add(transactionEntity);
                    _context.SaveChanges();
                    
                    if (category.ActionTypeId == (int)ActionTypeEnum.Expense)
                    {
                        card.Balance += transactionEntity.Amount;
                    }
                    else
                    {
                        card.Balance -= transactionEntity.Amount;
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
