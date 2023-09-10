using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<CardEntity> CardRepository { get; }
        IRepository<ColorEntity> ColorRepository { get; }
        IRepository<UserEntity> UserRepository { get; }
        IRepository<ActionTypeEntity> ActionTypeRepository { get; }
        IRepository<CategoryEntity> CategoryRepository { get; }
        IRepository<UserCategoryEntity> UserCategoryRepository { get; }

        Task<int> CommitAsync();
    }
}
