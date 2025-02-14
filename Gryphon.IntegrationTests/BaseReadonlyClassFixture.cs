using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests;

public class BaseReadonlyClassFixture : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    public readonly IMediator _mediator;
    public readonly IServiceScope _scope;

    public BaseReadonlyClassFixture(ReadonlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }
}
