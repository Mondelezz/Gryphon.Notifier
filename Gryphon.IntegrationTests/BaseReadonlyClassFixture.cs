using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests;

public class BaseReadonlyClassFixture : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    protected internal readonly IServiceScope _scope;

    public BaseReadonlyClassFixture(ReadonlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
    }
}
