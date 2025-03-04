using Application.Features.TopicFeatures.Command;
using Application.Features.TopicFeatures.Query;

namespace Gryphon.IntegrationTests.TopicFeaturesTests;

public sealed class CreateOrUpdateTopicTest : BaseWriteOnlyClassFixture
{
    public CreateOrUpdateTopicTest(WriteOnlyIntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Создание или обновление топика, когда идентификатор топика не указан, метод должен создать топик и вернуть его идентификатор
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateTopic_WhenTopicIdIsNull_MustCreateYopivAndReturnTopicId()
    {
        // Act
        CreateOrUpdateTopic.Command command = new(
            CurrentUserId: 1,
            TopicId: null,
            new CreateOrUpdateTopic.RequestDto(
                new CreateOrUpdateTopic.TopicDto(
                    Name: "Мероприятия",
                    TopicType: Domain.Enums.TopicType.Smart)));

        // Arrange
        long topicId = await _mediator.Send(command);

        // Assert
        Assert.True(topicId > 0);
    }

    /// <summary>
    /// Создание или обновление топика, когда идентификатор топика указан, метод должен обновить его и вернуть идентификатор
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateTopic_WhenTopicIdIsNotNull_MustUpdateTopicAndReturnTopicId()
    {
        // Act && Arrange
        CreateOrUpdateTopic.Command commandCreate = new(
            CurrentUserId: 1,
            TopicId: null,
            new CreateOrUpdateTopic.RequestDto(
                new CreateOrUpdateTopic.TopicDto(
                    Name: "Мероприятия",
                    TopicType: Domain.Enums.TopicType.Smart)));

        long topicCreatedId = await _mediator.Send(commandCreate);

        CreateOrUpdateTopic.Command commandUpdate = new(
            CurrentUserId: 1,
            TopicId: topicCreatedId,
            new CreateOrUpdateTopic.RequestDto(
                new CreateOrUpdateTopic.TopicDto(
                    Name: "Задачи",
                    TopicType: Domain.Enums.TopicType.Default)));

        long topicUpdatedId = await _mediator.Send(commandUpdate);

        GetTopic.ResponseDto updatedTopicDto = await _mediator.Send(new GetTopic.Query(commandUpdate.CurrentUserId, topicUpdatedId));

        // Assert
        Assert.NotNull(updatedTopicDto);
        Assert.NotNull(updatedTopicDto.TopicDto);

        Assert.Equal(topicCreatedId, topicUpdatedId);

        Assert.NotEqual(commandCreate.RequestDto.TopicDto.Name, updatedTopicDto.TopicDto.Name);
        Assert.NotEqual(commandCreate.RequestDto.TopicDto.TopicType, updatedTopicDto.TopicDto.TopicType);
    }
}
