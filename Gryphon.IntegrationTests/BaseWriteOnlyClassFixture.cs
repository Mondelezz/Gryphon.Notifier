using Mediator;

using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests;

public class BaseWriteOnlyClassFixture : IClassFixture<WriteOnlyIntegrationTestWebAppFactory>
{
    protected internal readonly IMediator _mediator;
    protected internal readonly IServiceScope _scope;

    public BaseWriteOnlyClassFixture(WriteOnlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }
}
