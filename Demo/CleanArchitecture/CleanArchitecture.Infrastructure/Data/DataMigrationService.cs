using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Domain.Models.FeatureFlag;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DataMigrationService<TContext> : IDataMigrationService where TContext : DbContext
    {
        private readonly ILogger<DataMigrationService<TContext>> _logger;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public DataMigrationService(IUnitOfWork<TContext> unitOfWork,
        ILogger<DataMigrationService<TContext>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start to migrate database");
            var context = _unitOfWork.Context;

            await context.Database.MigrateAsync();

            _logger.LogInformation("Migrage feature flag");

            string[] enableFeatureFlags = ["Event", "Resume", "Project"];

            string[] disabledFeatureFlags = ["Course", "Challenge"];

            var featureRepo = _unitOfWork.GetRepository<FeatureFlagEntity>();

            foreach (var item in enableFeatureFlags)
            {
                var feature = await featureRepo.FirstOrDefaultAsync(x => x.Name == item, cancellationToken);
                if (feature is null)
                {
                    featureRepo.Insert(new FeatureFlagEntity { Name = item, Enable = true });
                }
                else
                {
                    feature.Enable = true;
                }

            }

            foreach (var item in disabledFeatureFlags)
            {
                var feature = await featureRepo.FirstOrDefaultAsync(x => x.Name == item, cancellationToken);
                if (feature is null)
                {
                    featureRepo.Insert(new FeatureFlagEntity { Name = item, Enable = false });
                }
                else
                {
                    feature.Enable = false;
                }

            }

            await _unitOfWork.CommitAsync(cancellationToken);


            _logger.LogInformation("Completed migration");
        }
    }
}