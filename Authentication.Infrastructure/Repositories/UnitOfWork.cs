using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;

namespace Authentication.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<UserEntity> _users;
        private AuthenticationDbContext _dbContext;
        public UnitOfWork(AuthenticationDbContext dbContext) => _dbContext = dbContext;

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                return _users ??= new BaseRepository<UserEntity>(_dbContext);
            }
        }
    }
}
