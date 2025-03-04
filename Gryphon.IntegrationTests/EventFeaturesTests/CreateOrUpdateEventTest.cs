using Application.Features.EventFeatures.Command;
using Application.Features.EventFeatures.Query;

namespace Gryphon.IntegrationTests.EventFeaturesTests;

public sealed class CreateOrUpdateEventTest : BaseWriteOnlyClassFixture
{
    public CreateOrUpdateEventTest(WriteOnlyIntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Создание или обновление события, когда идентификатор события не указан, метод должен создать событие и вернуть идентификатор события
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateEvent_WhenEventIdIsNull_MustCreateEventAndReturnEventId()
    {
        // Act
        CreateOrUpdateEvent.Command command = new(
            new CreateOrUpdateEvent.RequestDto(
                new CreateOrUpdateEvent.EventDto(
                    Name: "Встреча с семьёй",
                    Description: "Встречаемся в кафе Миндаль",
                    Importance: Domain.Enums.Importance.High,
                    DateEvent: DateTime.UtcNow.AddDays(3).AddHours(5),
                    TimeEventStart: null,
                    TimeEventEnded: null,
                    Price: null,
                    IsIterative: false)),
            EventId: null,
            TopicId: null,
            CurrentUserId: 1);

        // Arrange
        long eventId = await _mediator.Send(command);

        // Assert
        Assert.True(eventId > 0);
    }

    /// <summary>
    /// Создание или обновление события, когда идентификатор события указан, метод должен обновить событие и вернуть идентификатор события
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateEvent_WhenEventIdIsNotNull_MustUpdateEventAndReturnEventId()
    {
        // Act && Arrange
        CreateOrUpdateEvent.Command commandCreate = new(
            new CreateOrUpdateEvent.RequestDto(
                new CreateOrUpdateEvent.EventDto(
                    Name: "Встреча с семьёй",
                    Description: "Встречаемся в кафе Миндаль",
                    Importance: Domain.Enums.Importance.High,
                    DateEvent: DateTime.UtcNow.AddDays(3).AddHours(5),
                    TimeEventStart: null,
                    TimeEventEnded: null,
                    Price: null,
                    IsIterative: false)),
            EventId: null,
            TopicId: null,
            CurrentUserId: 1);

        long eventCreatedId = await _mediator.Send(commandCreate);

        CreateOrUpdateEvent.Command commandUpdate = new(
            new CreateOrUpdateEvent.RequestDto(
                new CreateOrUpdateEvent.EventDto(
                    Name: "Встреча с друзьями",
                    Description: "Встречаемся в кафе Миндаль",
                    Importance: Domain.Enums.Importance.High,
                    DateEvent: DateTime.UtcNow.AddDays(3).AddHours(5),
                    TimeEventStart: null,
                    TimeEventEnded: null,
                    Price: null,
                    IsIterative: false)),
            EventId: eventCreatedId,
            TopicId: null,
            CurrentUserId: 1);

        long eventUpdatedId = await _mediator.Send(commandUpdate);

        GetEvent.ResponseDto updatedEventDto = await _mediator.Send(new GetEvent.Query(commandUpdate.CurrentUserId, eventUpdatedId));

        // Assert
        Assert.NotNull(updatedEventDto);
        Assert.NotNull(updatedEventDto.EventDto);

        Assert.Equal(eventCreatedId, eventUpdatedId);

        Assert.NotEqual(commandCreate.RequestDto.EventDto.Name, updatedEventDto.EventDto.Name); // Потому что обновляли только название

        Assert.Equal(commandCreate.RequestDto.EventDto.Description, updatedEventDto.EventDto.Description);
        Assert.Equal(commandCreate.RequestDto.EventDto.Importance, updatedEventDto.EventDto.Importance);
        Assert.Equal(commandCreate.RequestDto.EventDto.IsIterative, updatedEventDto.EventDto.IsIterative);
    }
}
