using ExpenseTracker.Application.Common.Dtos.Cards;
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
            //Test data
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
                    new CategoryEntity{ Id = 1, CategoryName = "Продукты", ImageUri = "https://img.icons8.com/fluency/64/fast-moving-consumer-goods.png", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 2, CategoryName = "Автомобиль", ImageUri = "https://img.icons8.com/fluency/64/sedan.png", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 3, CategoryName = "Образование", ImageUri = "https://img.icons8.com/fluency/64/sedan.png", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 4, CategoryName = "Недвижимость", ImageUri = "https://img.icons8.com/fluency/64/real-estate.png", ActionTypeId = 1, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 5, CategoryName = "Зарплата", ImageUri = "https://img.icons8.com/fluency/64/money-transfer.png", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 6, CategoryName = "Инвестиции", ImageUri = "https://img.icons8.com/fluency/64/economic-improvement.png", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                    new CategoryEntity{ Id = 7, CategoryName = "Подарок", ImageUri = "https://img.icons8.com/fluency/64/gift--v1.png", ActionTypeId = 2, CreatedOnUtc= DateTime.Now},
                });

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity[]
                {
                    new UserEntity{ Id = 1, Login = "Admin", Password = "Admin1$1", CreatedOnUtc = DateTime.Now }
                });
            modelBuilder.Entity<CardEntity>().HasData(
                new CardEntity[]
                {
                    new CardEntity { Id = 1, Balance = 600, ColorId = 2, CardName = "Кошелек 1", UserId = 1, CreatedOnUtc = DateTime.Now},
                    new CardEntity { Id = 2, Balance = 400, ColorId = 1, CardName = "Кошелек 2", UserId = 1, CreatedOnUtc = DateTime.Now},
                });
            modelBuilder.Entity<UserCategoryEntity>().HasData(
                new UserCategoryEntity[]
                {
                    new UserCategoryEntity { Id = 1, UserId = 1, CategoryId = 1, CreatedOnUtc = DateTime.Now},
                    new UserCategoryEntity { Id = 2, UserId = 1, CategoryId = 2, CreatedOnUtc = DateTime.Now},
                    new UserCategoryEntity { Id = 3, UserId = 1, CategoryId = 3, CreatedOnUtc = DateTime.Now},
                    new UserCategoryEntity { Id = 4, UserId = 1, CategoryId = 5, CreatedOnUtc = DateTime.Now},
                    new UserCategoryEntity { Id = 5, UserId = 1, CategoryId = 6, CreatedOnUtc = DateTime.Now}
                });
            modelBuilder.Entity<TransactionEntity>().HasData(
                new TransactionEntity[]
                {
                    new TransactionEntity { Id = 1, Amount = 35, CardId = 1, CategoryId = 1, Comment = "Магазин Магнолия", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                    new TransactionEntity { Id = 2, Amount = 85, CardId = 1, CategoryId = 1, Comment = "Магазин Пятерочка", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                    new TransactionEntity { Id = 3, Amount = 400, CardId = 1, CategoryId = 4, Comment = "Оплата общежития", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                    new TransactionEntity { Id = 4, Amount = 400, CardId = 1, CategoryId = 5, Comment = "Зарплата", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                    new TransactionEntity { Id = 5, Amount = 120, CardId = 2, CategoryId = 1, Comment = "Магазин Перекресток", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                    new TransactionEntity { Id = 6, Amount = 300, CardId = 2, CategoryId = 7, Comment = "Подарок от родителей", Date = DateTime.Now, CreatedOnUtc = DateTime.Now},
                });
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
