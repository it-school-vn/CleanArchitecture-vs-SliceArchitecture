using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Domain.Models.FeatureFlag;
using CleanArchitecture.Domain.Models.FeatureFlag.DTO;

namespace CleanArchitecture.Application.Features.FeatureFlag.Queries;

public static class ListFeatureFlag
{

    public class Query : IQuery<OperationResult<Pagination<FeatureFlagResponse>>>
    { }

    public class Handler : IQueryHandler<Query, OperationResult<Pagination<FeatureFlagResponse>>>
    {
        private readonly IGenericService<FeatureFlagEntity> _featureFlagService;
        public Handler(IGenericService<FeatureFlagEntity> featureFlagService)
        {
            _featureFlagService = featureFlagService;
        }

        public async Task<OperationResult<Pagination<FeatureFlagResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var items = await _featureFlagService.ListByAsync<FeatureFlagResponse>(x => new FeatureFlagResponse(x.Name, x.Enable), default, x => x.OrderBy(f => f.Name), default, false, 1, Global.PageSize, cancellationToken);

            return OperationResult.Ok(items);
        }
    }
}