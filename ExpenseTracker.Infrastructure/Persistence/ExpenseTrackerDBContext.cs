using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class ExpenseTrackerDBContext : DbContext, IExpenseTrackerDBContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserCategoryEntity> UserCategories { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<ColorEntity> Colors { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<ActionTypeEntity> Actions { get; set; }

        public DbSet<ErrorLogEntity> ErrorLogs { get; set; }
        public ExpenseTrackerDBContext(DbContextOptions<ExpenseTrackerDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColorEntity>().HasData(
                new ColorEntity[]
                {
                    new ColorEntity { Id = 1, ColorName="Green", CreatedOnUtc = DateTime.Now },
                    new ColorEntity { Id = 2, ColorName="Blue", CreatedOnUtc = DateTime.Now },
                    new ColorEntity { Id = 3, ColorName="White", CreatedOnUtc = DateTime.Now },
                });

            modelBuilder.Entity<ActionTypeEntity>().HasData(
                new ActionTypeEntity[]
                {
                    new ActionTypeEntity{ Id = 1, ActionTypeName = "Expense", CreatedOnUtc= DateTime.Now},
                    new ActionTypeEntity{ Id = 2, ActionTypeName = "Income", CreatedOnUtc= DateTime.Now}
                });

            modelBuilder.Entity<CategoryEntity>().HasData(
                new CategoryEntity[]
                {
                    new CategoryEntity{ Id = 1, CategoryName = "Продукты", ImageUri = "uri", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 2, CategoryName = "Автомобиль", ImageUri = "uri", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 3, CategoryName = "Образование", ImageUri = "uri", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 4, CategoryName = "Недвижимость", ImageUri = "uri", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 5, CategoryName = "Зарплата", ImageUri = "uri", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 6, CategoryName = "Инвестиции", ImageUri = "uri", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 7, CategoryName = "Подарок", ImageUri = "uri", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                });
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
