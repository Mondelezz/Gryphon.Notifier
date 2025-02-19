using Application.Exceptions;
using Application.Features.EventFeatures.Query;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public sealed class GetEventTest : BaseReadonlyClassFixture
{
    public GetEventTest(ReadonlyIntegrationTestWebAppFactory factory) : base(factory)
    {
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
