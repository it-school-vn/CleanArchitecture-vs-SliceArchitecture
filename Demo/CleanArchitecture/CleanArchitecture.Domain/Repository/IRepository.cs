using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IDeleteRepository<T>, IDisposable where T : class
    {

        T Insert(T entity);
        Task InsertAsync(params T[] entities);
        Task InsertAsync(IEnumerable<T> entities);

        Task<T> InsertIfNotExistsAsync(Expression<Func<T, bool>> predicate, T entity);

        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);

    }
}