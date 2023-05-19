using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public interface IRepository<TEntity> : IDisposable
    {
        void Remove(TEntity row);
        void RemoveRange(List<TEntity> rows);
        void Add(TEntity row);
        void AddRange(List<TEntity> rows);
        void SaveChanges();
        List<TResult> GetMany<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            bool distinct = false
        );
        List<TEntity> GetMany(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false
        );
        TEntity? FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            params string[] includeProperties
        );
        TResult? FirstOrDefault<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select
        );
        bool Exists(Expression<Func<TEntity, bool>> predicate);
    }
}
