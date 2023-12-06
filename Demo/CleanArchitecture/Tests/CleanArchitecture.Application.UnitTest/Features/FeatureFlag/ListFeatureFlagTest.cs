using NSubstitute;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Domain.Models.FeatureFlag;
using CleanArchitecture.Domain.Models.FeatureFlag.DTO;
using static CleanArchitecture.Application.Features.FeatureFlag.Queries.ListFeatureFlag;

namespace CleanArchitecture.Application.UnitTest.Features.FeatureFlag;

public class FeatureFlagTest
{
    private IGenericService<FeatureFlagEntity> _featureFlagService;

    private Handler _handler;

    [SetUp]
    public void Setup()
    {
        _featureFlagService = Substitute.For<IGenericService<FeatureFlagEntity>>();
        _handler = new Handler(_featureFlagService);
    }

    [Test]
    public async Task Handle_ReturnsSuccess_WhenFeatureFlagsAreFound()
    {
        // Arrange
        var items = new List<FeatureFlagEntity>
            {
                new() { Name = "Flag1", Enable = true },
                new() { Name = "Flag2", Enable = false }
            };

        var queryResult = new Pagination<FeatureFlagResponse>(items.Select(x => new FeatureFlagResponse(x.Name, x.Enable)).ToList(), 2, 1, Global.PageSize);


        _featureFlagService.ListByAsync<FeatureFlagResponse>(
            x => new FeatureFlagResponse(x.Name, x.Enable),
            default,
            x => x.OrderBy(f => f.Name),
            default,
            false,
            1,
            Global.PageSize)
            .ReturnsForAnyArgs(Task.FromResult(queryResult));

        //Act

        var result = await _handler.Handle(new Query(), CancellationToken.None);

        //Assert

        Assert.That(result, Is.InstanceOf<OperationResult<Pagination<FeatureFlagResponse>>>());

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Value, Is.Not.Null);

        Assert.That(result.Value.Data.Count(), Is.EqualTo(items.Count));
    }
}
