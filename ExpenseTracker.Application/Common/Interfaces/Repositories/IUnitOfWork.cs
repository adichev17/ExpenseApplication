using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<CardEntity> CardRepository { get; }
        IRepository<ColorEntity> ColorRepository { get; }
        IRepository<UserEntity> UserRepository { get; }

        Task<int> CommitAsync();
    }
}
