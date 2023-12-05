using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public sealed class Repository<T> : ReadRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #region Insert Functions

        public T Insert(T entity)
        {
            if (entity is BaseEntity<Ulid> ulidEntity)
            {
                ulidEntity.Id = Ulid.NewUlid();
            }

            return _dbSet.Add(entity).Entity;
        }

        public Task InsertAsync(params T[] entities)
        {
            return _dbSet.AddRangeAsync(entities);
        }

        public Task InsertAsync(IEnumerable<T> entities)
        {
            return _dbSet.AddRangeAsync(entities);
        }

        public async Task<T> InsertIfNotExistsAsync(Expression<Func<T, bool>> predicate, T entity)
        {
            if (predicate is not null && _dbSet.Any(predicate))
            {
                var result = _dbSet.SingleOrDefault(predicate.Compile());

                if (result != null)
                {
                    return result;
                }
            }

            await _dbSet.AddAsync(entity);

            return entity;
        }

        #endregion


        #region Update Functions

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);

        }

        public void Update(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<T> entities)
        {

            _dbSet.UpdateRange(entities);
        }

        #endregion
    }
}