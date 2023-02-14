using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class ExpenseTrackerDBContext : DbContext, IExpenseTrackerDBContext
    {
        public ExpenseTrackerDBContext(DbContextOptions<ExpenseTrackerDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserCategoryEntity> UserCategories { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<ColorEntity> Colors { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<ActionTypeEntity> Actions { get; set; }

        
        public DbSet<ErrorLogEntity> ErrorLogs { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
