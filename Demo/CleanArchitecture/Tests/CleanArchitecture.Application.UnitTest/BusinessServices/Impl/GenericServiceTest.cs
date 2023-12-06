using NSubstitute;
using CleanArchitecture.Application.BusinessServices.Impl;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.UnitTest.BusinessServices.Impl;
public class GenericServiceTest
{
    public class FakeEntity : IEntity
    {

    };

    public class FakeNonEntity
    {

    };
    private IUnitOfWork _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork?.Dispose();
    }

    [Test]
    public async Task CreateNewAsync_NullArgument_ThrowNullArgumentException()
    {
        // Arrange

        var service = new GenericService<FakeEntity>(_unitOfWork);

        FakeEntity? entity = default;

        // Action

        // Assert
#pragma warning disable CS8604 
        await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateNewAsync(entity, default)));
#pragma warning restore CS8604 

    }
}
