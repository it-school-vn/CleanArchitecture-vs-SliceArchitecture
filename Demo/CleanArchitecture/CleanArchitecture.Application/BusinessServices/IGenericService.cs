using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.BusinessServices;

public interface IGenericService<T> where T : class
{
    Task<T> CreateNewAsync(T item, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T newItem, CancellationToken cancellationToken);

    Task<T> InsertAsync(T newItem, CancellationToken cancellationToken);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate, bool enableTracking, CancellationToken cancellationToken);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include, bool enableTracking, CancellationToken cancellationToken);
    Task<Pagination<T>> ListByAsync(Expression<Func<T, bool>>? predicate,
       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
       Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include,
        bool enableTracking = false,
        int page = 1,
        int size = Global.PageSize,
        CancellationToken cancellationToken = default);
    Task<Pagination<TResult>> ListByAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
         Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include,
         bool enableTracking = false,
         int page = 1,
         int size = Global.PageSize,
         CancellationToken cancellationToken = default) where TResult : class;

}
