using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public abstract class RepositoryBase<TEntity>: IRepository<TEntity> 
        where TEntity : class
    {
        protected readonly FileMonitorDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;
        private bool disposedValue;

        public RepositoryBase(FileMonitorDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public void Remove(TEntity row) 
            => _dbSet.Remove(row);

        public void RemoveRange(List<TEntity> rows) 
            => _dbSet.RemoveRange(rows);

        public void Add(TEntity row)
            => _dbSet.Add(row);

        public void AddRange(List<TEntity> rows)
            => _dbSet.AddRange(rows);

        public void SaveChanges()
            => _db.SaveChanges();

        public List<TResult> GetMany<TResult, TProperty>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, TProperty>> order = null,
            bool distinct = false
            )
        {
            var query = _dbSet.Where(predicate);

            if (order is not null)
                query = query.OrderBy(order);

            var projected = query.Select(select);

            if (distinct)
                projected = projected.Distinct();

            return projected.ToList();
        }

        public List<TEntity> GetMany(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false
            ) 
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public TEntity? FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            params string[] includeProperties
            )
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            foreach (string property in includeProperties)
                query = query.Include(property);

            if(asNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(predicate);
        }

        public TResult? FirstOrDefault<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select
            )
        {
            return _dbSet.Where(predicate).Select(select).FirstOrDefault();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.Any(predicate);

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
