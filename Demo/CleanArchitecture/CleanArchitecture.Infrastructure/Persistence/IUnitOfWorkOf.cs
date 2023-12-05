using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.Persistence;
public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : Microsoft.EntityFrameworkCore.DbContext
{
    TContext Context { get; }
}