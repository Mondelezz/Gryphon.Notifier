using Application.Features.EventFeatures.Query;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public sealed class GetListEventTest : BaseReadonlyClassFixture
{
    public GetListEventTest(ReadonlyIntegrationTestWebAppFactory factory) : base(factory)
    {
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
        Assert.True(result.EventDtos.All(e => e.Price > 0));
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
        Assert.True(result.EventDtos.All(e => e.IsCompleted));
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
        Assert.True(result.EventDtos.All(e => e.TopicDto!.TopicId == query.Filter!.TopicId));
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
        Assert.Equal(1, result.ActualEventsCount); // TODO: Поменять, если бэкап данных изменился
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
        Assert.Equal(5, result.EndedEventsCount); // TODO: Поменять, если бэкап данных изменился
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
        Assert.Equal(6, result.TotalCount); // TODO: Поменять, если бэкап данных изменился
    }

    [Fact]
    public async Task Handle_Calculates_TotalPrice_Events_InTopic_ReturnsCorrectResults()
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
        Assert.Equal(1002, result.TotalPrice); // TODO: Поменять, если бэкап данных изменился
    }
}
