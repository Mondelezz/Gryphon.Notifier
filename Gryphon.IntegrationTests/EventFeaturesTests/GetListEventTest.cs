using Mediator;
using Microsoft.Extensions.DependencyInjection;

using Application.Features.EventFeatures.Query;

using FluentAssertions;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public class GetListEventTest : IClassFixture<ReadonlyIntegrationTestWebAppFactory>
{
    private readonly IMediator _mediator;
    private readonly IServiceScope _scope;

    public GetListEventTest(ReadonlyIntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Handle_IndicatedPriceFilter_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter(IndicatedPriceFilter: true)
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EventDtos.Should().OnlyContain(e => e.Price > 0);
    }

    [Fact]
    public async Task Handle_IsCompletedFilter_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter(IsCompleted: true)
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EventDtos.Should().OnlyContain(e => e.IsCompleted);
    }

    [Fact]
    public async Task Handle_TopicIdFilter_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter(TopicId: 1)
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EventDtos.Should().OnlyContain(e => e.TopicDto!.TopicId == query.Filter!.TopicId);
    }

    [Fact]
    public async Task Handle_Calculates_ActualEventsCount_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter()
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.ActualEventsCount.Should().Be(1); // TODO: Поменять, если бэкап данных изменился
    }

    [Fact]
    public async Task Handle_Calculates_EndedEventsCount_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter()
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.EndedEventsCount.Should().Be(5); // TODO: Поменять, если бэкап данных изменился
    }

    [Fact]
    public async Task Handle_Calculates_TotalCount_Events_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter()
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.TotalCount.Should().Be(6); // TODO: Поменять, если бэкап данных изменился
    }

    [Fact]
    public async Task Handle_Calculates_TotalPrice_Events_InGroup_ReturnsCorrectResults()
    {
        // Arrange
        GetListEvent.Query query = new(
            UserId: "1",
            Offset: 10,
            SkipCount: 0,
            Sorting: GetListEvent.Sorting.Id,
            SortByDescending: false,
            Filter: new GetListEvent.RequestFilter(TopicId: 1)
        );

        // Act
        GetListEvent.ResponseDto result = await _mediator.Send(query);

        // Assert
        result.TotalPrice.Should().Be(1002); // TODO: Поменять, если бэкап данных изменился
    }
}
