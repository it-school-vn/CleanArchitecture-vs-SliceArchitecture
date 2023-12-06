using Mono.Cecil;
using NetArchTest.Rules;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Architecture.Test;

public class DomainLayerTest
{

    [Test]
    public void DomainLayer_ShouldNotDependenOn_InfrastructureAndApplication()
    {
        var testResult = Types
        .InAssembly(typeof(Domain.Startup).Assembly)
        .ShouldNot()
        .HaveDependencyOnAll("Application", "Infrastructure")
        .GetResult();

        Assert.True(testResult.IsSuccessful);
    }

    [Test]
    public void Entity_ShouldHaveNameEndWithEntity()
    {
        var testResult = Types
        .InAssembly(typeof(Domain.Startup).Assembly)
        .That()
        .Inherit(typeof(BaseTimestampEntity<>))
        .Should()
        .HaveNameEndingWith("Entity")
        .GetResult();

        Assert.True(testResult.IsSuccessful);
    }

    [Test]
    public void Entity_ShouldBeSealed()
    {
        var testResult = Types
        .InAssembly(typeof(Domain.Startup).Assembly)
        .That()
        .Inherit(typeof(BaseTimestampEntity<>))
        .Should()
        .BeSealed()
        .GetResult();

        Assert.True(testResult.IsSuccessful);
    }


}