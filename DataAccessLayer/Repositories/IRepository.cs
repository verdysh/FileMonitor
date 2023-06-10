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
        List<TResult> GetRange<TResult, TProperty>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, TProperty>> order = null,
            bool distinct = false
        );
        List<TEntity> GetRange(
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
