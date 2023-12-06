using NetArchTest.Rules;

namespace CleanArchitecture.Architecture.Test;

public class ApplicationLayerTest
{

    [Test]
    public void ApplicationLayer_ShouldNotDependenOn_Infrastructure()
    {
        var testResult = Types
        .InAssembly(typeof(Application.Startup).Assembly)
        .ShouldNot()
        .HaveDependencyOnAll("Infrastructure")
        .GetResult();

        Assert.True(testResult.IsSuccessful);
    }


}