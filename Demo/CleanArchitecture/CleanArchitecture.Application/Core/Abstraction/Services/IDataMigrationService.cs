namespace CleanArchitecture.Application.Core.Abstraction.Services
{
    public interface IDataMigrationService : IDisposable
    {
        Task MigrateAsync(CancellationToken cancellationToken);
    }
}