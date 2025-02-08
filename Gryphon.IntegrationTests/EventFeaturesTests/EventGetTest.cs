using Application.Exceptions;
using Application.Features.EventFeatures.Query;

using Mediator;

using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public class EventGetTest : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    private readonly IMediator _mediator;
    private readonly IServiceScope _scope;

    public EventGetTest(ReadonlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetEventById_ShouldReturnEvent_WhenEventExist(long eventId)
    {
        // Arrange
        GetEvent.Query query = new("1", eventId);

        // Act
        GetEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.EventDto.EventId, eventId);
    }

    [Theory]
    [InlineData(6666666666)]
    [InlineData(6666666667)]
    public async Task GetEventById_ShouldReturnError_WhenEventNotExist(long eventId)
    {
        // Arrange
        GetEvent.Query query = new("1", eventId);

        // Act && Assert
        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _mediator.Send(request: query));
    }
}
