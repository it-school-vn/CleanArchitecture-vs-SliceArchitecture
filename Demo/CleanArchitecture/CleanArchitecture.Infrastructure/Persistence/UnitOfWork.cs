using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext, IDisposable
    {
        private Dictionary<(Type type, string name), object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = [];
        }

        public TContext Context { get; }


        public Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IDeleteRepository<TEntity> GetDeleteRepository<TEntity>() where TEntity : class
        {
            return (IDeleteRepository<TEntity>)GetOrAddRepository(typeof(TEntity),
                new DeleteRepository<TEntity>(Context));
        }

        public IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class
        {
            return (IReadRepository<TEntity>)GetOrAddRepository(typeof(TEntity),
                new ReadRepository<TEntity>(Context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IRepository<TEntity>)GetOrAddRepository(typeof(TEntity), new Repository<TEntity>(Context));
        }
        internal object GetOrAddRepository(Type type, object repo)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (repo is null)
            {
                throw new ArgumentNullException(nameof(repo));
            }

            _repositories ??= new Dictionary<(Type type, string Name), object>();

            var name = repo.GetType().FullName;

            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException("repo.GetType().FullName");
            }

            if (_repositories.TryGetValue((type, name), out var repository))
            {
                return repository;
            }

            _repositories.Add((type, name), repo);

            return repo;
        }
    }
}