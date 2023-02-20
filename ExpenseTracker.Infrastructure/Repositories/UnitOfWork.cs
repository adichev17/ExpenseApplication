using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<CardEntity> _cards;
        private BaseRepository<ColorEntity> _colors;
        private BaseRepository<UserEntity> _users;
        private ExpenseTrackerDBContext _dbContext;
        public UnitOfWork(ExpenseTrackerDBContext dbContext) => _dbContext = dbContext;

        public IRepository<CardEntity> CardRepository
        {
            get
            {
                return _cards ??= new BaseRepository<CardEntity>(_dbContext);
            }
        }

        public IRepository<ColorEntity> ColorRepository
        {
            get
            {
                return _colors ??= new BaseRepository<ColorEntity>(_dbContext);
            }
        }

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                return _users ??= new BaseRepository<UserEntity>(_dbContext);
            }
        }

        public Task<int> CommitAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
