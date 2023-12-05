using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Domain.Repositories;

#nullable disable
namespace CleanArchitecture.Infrastructure.Persistence
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public ReadRepository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<T>();

        }
        #region AsQueryAble
        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null)
        {
            return AsQueryable(predicate, default);
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            return AsQueryable(predicate, orderBy, default);
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return AsQueryable(predicate, orderBy, include, default);
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = false)
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query)
                : query;

        }
        #endregion

        #region FirstOrDefault
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default)
        {
            return FirstOrDefaultAsync(predicate, default, cancellation);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, CancellationToken cancellation = default)
        {
            return FirstOrDefaultAsync(predicate, orderBy, default, cancellation);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellation = default)
        {
            return FirstOrDefaultAsync(predicate, orderBy, include, default, cancellation);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool enableTracking = false, CancellationToken cancellation = default)
        {
            return FirstOrDefaultAsync(predicate, orderBy, include, enableTracking, default, cancellation);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = false, bool ignoreQueryFilters = false, CancellationToken cancellation = default)
        {
            return FirstOrDefaultAsync(x => x, predicate, orderBy, include, enableTracking, ignoreQueryFilters, cancellation);
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         bool enableTracking = false,
         bool ignoreQueryFilters = false,
         CancellationToken cancellation = default) where TResult : class
        {
            IQueryable<T> query = _dbSet;

            if (!enableTracking) query = query.AsNoTracking();


            if (include != null) query = include(query).AsSplitQuery();


            if (predicate != null) query = query.Where(predicate);


            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return orderBy != null ? orderBy(query).Select(selector).FirstOrDefaultAsync(cancellation) :
                query.Select(selector).FirstOrDefaultAsync(cancellation);

        }


        #endregion

        public Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default)
        {
            return GetListAsync(predicate, default, cancellation);
        }

        public Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken cancellation = default)
        {
            return GetListAsync(predicate, orderBy, default, cancellation);
        }

        public Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
         IOrderedQueryable<T>> orderBy, Func<IQueryable<T>,
          IIncludableQueryable<T, object>> include = null, CancellationToken cancellation = default)
        {
            return GetListAsync(predicate, orderBy, include, 1, Global.PageSize, default, default, cancellation);

        }

        public Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
          int page = 1, int size = 20, bool enableTracking = false, bool ignoreQueryFilters = false, CancellationToken cancellation = default)
        {
            return GetListAsync<T>(x => x, predicate, orderBy, include, page, size, enableTracking, ignoreQueryFilters, cancellation);
        }

        public async Task<Pagination<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        int page = 1,
        int size = 20,
        bool enableTracking = false, bool ignoreQueryFilters = false, CancellationToken cancellation = default) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query).AsSplitQuery();
            if (predicate != null) query = query.Where(predicate);
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            var index = page > 0 ? page - 1 : 0;

            var resultTask = await (orderBy != null
                ? orderBy(query).Select(selector).Skip(index * size).Take(size)
                : query.Select(selector).Skip(index * size).Take(size)).ToListAsync(cancellation);

            var totalTask = await query.CountAsync(cancellation);

            return new Pagination<TResult>(resultTask, totalTask, page, size);
        }
    }
}