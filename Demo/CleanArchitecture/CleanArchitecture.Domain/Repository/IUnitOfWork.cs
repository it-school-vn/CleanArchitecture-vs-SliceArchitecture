namespace CleanArchitecture.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class;

        IDeleteRepository<TEntity> GetDeleteRepository<TEntity>() where TEntity : class;

        Task<int> CommitAsync(CancellationToken cancellationToken);
    }


}