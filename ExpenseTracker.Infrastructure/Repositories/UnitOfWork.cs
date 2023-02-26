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
        private BaseRepository<ActionTypeEntity> _actions;
        private BaseRepository<CategoryEntity> _categories;
        private BaseRepository<UserCategoryEntity> _userCategories;
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

        public IRepository<ActionTypeEntity> ActionTypeRepository
        {
            get
            {
                return _actions ??= new BaseRepository<ActionTypeEntity>(_dbContext);
            }
        }

        public IRepository<CategoryEntity> CategoryRepository
        {
            get
            {
                return _categories ??= new BaseRepository<CategoryEntity>(_dbContext);
            }
        }

        public IRepository<UserCategoryEntity> UserCategoryRepository
        {
            get
            {
                return _userCategories ??= new BaseRepository<UserCategoryEntity>(_dbContext);
            }
        }

        

        public Task<int> CommitAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
