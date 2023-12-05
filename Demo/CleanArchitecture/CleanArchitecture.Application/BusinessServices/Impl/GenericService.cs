using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.BusinessServices.Impl;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly IUnitOfWork _unitOfWork;

    public GenericService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ValidateEntity();

    }

    private static void ValidateEntity()
    {
        var tType = typeof(T);

        if (!typeof(IEntity).IsAssignableFrom(tType))
        {
            throw new InvalidEntityException(tType.Name);
        }
    }

    protected IRepository<T> Repository => _unitOfWork.GetRepository<T>();
    public virtual async Task<T> CreateNewAsync(T item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        var insertedItem = Repository.Insert(item);

        await _unitOfWork.CommitAsync(cancellationToken);

        return insertedItem;
    }

    public Task<int> UpdateAsync(T newItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(newItem);

        Repository.Update(newItem);

        return _unitOfWork.CommitAsync(cancellationToken);

    }

    public async Task<T> InsertAsync(T newItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(newItem);

        var insertedItem = Repository.Insert(newItem);

        await _unitOfWork.CommitAsync(cancellationToken);

        return insertedItem;
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return Repository.AsQueryable(predicate).AsNoTracking().CountAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return Repository.AsQueryable(predicate).AsNoTracking().AnyAsync(cancellationToken);
    }

    public Task<T> GetByAsync(Expression<Func<T, bool>> predicate, bool enableTracking, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return Repository.FirstOrDefaultAsync(predicate, enableTracking: enableTracking, cancellation: cancellationToken);

    }
    public Task<T> GetByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include, bool enableTracking, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return Repository.FirstOrDefaultAsync(predicate, include: include, enableTracking: enableTracking, cancellation: cancellationToken);
    }

    public Task<Pagination<T>> ListByAsync(Expression<Func<T, bool>>? predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
    Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include,
     bool enableTracking = false,
     int page = 1,
     int size = Global.PageSize,
     CancellationToken cancellationToken = default)
    {
        return Repository.GetListAsync(predicate,
        orderBy: orderBy,
        include: include,
        page: page,
        size: size,
        enableTracking: enableTracking,
        cancellation: cancellationToken);
    }

    public Task<Pagination<TResult>> ListByAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate,
       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
       Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include,
        bool enableTracking = false,
        int page = 1,
        int size = Global.PageSize,
        CancellationToken cancellationToken = default) where TResult : class

    {
        return Repository.GetListAsync(selector, predicate,
        orderBy: orderBy,
        include: include,
        page: page,
        size: size,
        enableTracking: enableTracking,
        cancellation: cancellationToken);
    }

}
