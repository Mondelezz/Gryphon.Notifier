using Application.Exceptions;
using Application.Features.EventFeatures.Query;

using Mediator;

using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public class EventGetTest(ReadonlyIntegrationTestWebAppFactory factory) : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    private async Task GetEventById_ShouldReturnEvent_WhenEventExist(long eventId)
    {
        // Arrange
        using IServiceScope scope = factory.Services.CreateScope();

        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        EventGet.Query query = new("1", eventId);

        // Act
        EventGet.ResponseDto result = await mediator.Send(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.EventDto.EventId, eventId);
    }

    [Theory]
    [InlineData(6666666666)]
    [InlineData(6666666667)]
    private async Task GetEventById_ShouldReturnError_WhenEventNotExist(long eventId)
    {
        // Arrange
        using IServiceScope scope = factory.Services.CreateScope();

        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        EventGet.Query query = new("1", eventId);

        // Act && Assert
        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await mediator.Send(request: query));
    }
}
