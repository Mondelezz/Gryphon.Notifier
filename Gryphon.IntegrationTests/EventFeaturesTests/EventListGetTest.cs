using Mediator;
using Microsoft.Extensions.DependencyInjection;

using Application.Features.EventFeatures.Query;

using FluentAssertions;

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
        result.EventDtos.Should().OnlyContain(e => e.Price > 0);
    }

    [Fact]
    public async Task Handle_IsCompletedFilter_ReturnsCorrectResults()
    {
        // Arrange
        EventListGet.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: EventListGet.Sorting.Id,
            SortByDescending: false,
            Filter: new EventListGet.RequestFilter(IsCompleted: true)
        );

        // Act
        EventListGet.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EventDtos.Should().OnlyContain(e => e.IsCompleted);
    }

    [Fact]
    public async Task Handle_GeroupEventIdFilter_ReturnsCorrectResults()
    {
        // Arrange
        EventListGet.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: EventListGet.Sorting.Id,
            SortByDescending: false,
            Filter: new EventListGet.RequestFilter(GroupEventId: 1)
        );

        // Act
        EventListGet.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EventDtos.Should().OnlyContain(e => e.GroupEventDto!.GroupEventId == query.Filter!.GroupEventId);
    }

    [Fact]
    public async Task Handle_CalculatesActualEventsCount_()
    {
        // Arrange
        EventListGet.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: EventListGet.Sorting.Id,
            SortByDescending: false,
            Filter: new EventListGet.RequestFilter()
        );

        // Act
        EventListGet.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.ActualEventsCount.Should().Be(1); // TODO: Поменять, если бэкап данных изменился
    }
}
