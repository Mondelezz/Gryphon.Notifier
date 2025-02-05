using Mediator;
using Microsoft.Extensions.DependencyInjection;

using Application.Features.EventFeatures.Query;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public class EventListGetTest : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    private readonly IMediator _mediator;
    private readonly IServiceScope _scope;

    public EventListGetTest(ReadonlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Handle_IndicatedPriceFilter_ReturnsCorrectResults()
    {
        // Arrange
        EventListGet.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: EventListGet.Sorting.Id,
            SortByDescending: false,
            Filter: new EventListGet.RequestFilter(IndicatedPriceFilter: true)
        );

        // Act
        EventListGet.ResponseDto result = await _mediator.Send(query);

        // Assert
        Assert.True(result.EventDtos.All(e => e.Price > 0));
    }
}
