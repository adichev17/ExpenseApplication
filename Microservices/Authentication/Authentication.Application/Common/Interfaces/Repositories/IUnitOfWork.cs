using Authentication.Domain.Entities;

namespace Authentication.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<UserEntity> UserRepository { get; }

        Task<int> CommitAsync();
    }
}
