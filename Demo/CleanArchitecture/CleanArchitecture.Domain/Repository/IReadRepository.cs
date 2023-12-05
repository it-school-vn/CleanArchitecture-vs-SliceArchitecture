using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;

#nullable disable
namespace CleanArchitecture.Domain.Repositories
{
  public interface IReadRepository<T> where T : class
  {

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, CancellationToken cancellation = default);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
     IOrderedQueryable<T>> orderBy,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
    CancellationToken cancellation = default);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
     Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool enableTracking = false, CancellationToken cancellation = default);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = false, bool ignoreQueryFilters = false, CancellationToken cancellation = default);

    Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector,
     Expression<Func<T, bool>> predicate = null,
     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
     bool enableTracking = false,
     bool ignoreQueryFilters = false, CancellationToken cancellation = default) where TResult : class;

    Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default);
    Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken cancellation = default);

    Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellation = default);

    Task<Pagination<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
       int page = 1,
       int size = Global.PageSize, bool enableTracking = false, bool ignoreQueryFilters = false, CancellationToken cancellation = default);

    public Task<Pagination<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector,
       Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
       int page = 1, int size = Global.PageSize,
        bool enableTracking = false, bool ignoreQueryFilters = false,
        CancellationToken cancellation = default) where TResult : class;

    IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null);
    IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

    IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null,
   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate = null,
  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = false);

  }
}