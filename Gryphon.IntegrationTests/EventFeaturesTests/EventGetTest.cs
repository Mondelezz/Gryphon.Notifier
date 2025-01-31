using Application.Features.EventFeatures.Query.EventGet;

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
        Assert.Equal(eventId, result.EventDto.EventId);
    }
}
