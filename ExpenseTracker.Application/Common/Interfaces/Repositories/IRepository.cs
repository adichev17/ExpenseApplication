using System.Linq.Expressions;

namespace ExpenseTracker.Application.Common.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        IEnumerable<TEntity> GetAll();
        void Delete(object id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByIdAsync(object id);
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
    }
}
