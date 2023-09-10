using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var card = _context.Cards.FirstOrDefault(x => x.Id == transactionEntity.CardId);
                var category = _context.Categories.FirstOrDefault(x => x.Id == transactionEntity.CategoryId);

                if (card is null)
                    throw new NullReferenceException(nameof(card));
                if (category is null)
                    throw new NullReferenceException(nameof(category));

                _context.Transactions.Add(transactionEntity);
                _context.SaveChanges();

                if (category.ActionTypeId == (int)ActionTypeEnum.Expense)
                {
                    card.Balance -= transactionEntity.Amount;
                }
                else
                {
                    card.Balance += transactionEntity.Amount;
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

        public IQueryable<TransactionEntity> GetAll(Guid userId, int cardId = 0, int rows = 100)
        {
            var transactions = _context.Transactions
                .Include(x => x.Card)
                .Include(x => x.Category)
                .Where(x => x.Card.UserId == userId);

            if (cardId is not 0)
            {
                transactions = transactions.Where(x => x.CardId == cardId);
            }

            transactions = transactions.OrderByDescending(x => x.Id).Take(rows);
            return transactions;
        }

        public async Task<TransactionEntity> GetByIdAsync(int transactionId)
        {
            var transaction = await _context.Transactions
                .Include(x=> x.Card)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == transactionId);
            return transaction;
        }
    }
}
