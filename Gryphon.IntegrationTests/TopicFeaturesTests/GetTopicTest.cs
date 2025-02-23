using Application.Exceptions;
using Application.Features.TopicFeatures.Query;

namespace Gryphon.IntegrationTests.TopicFeaturesTests;

public sealed class GetTopicTest : BaseReadonlyClassFixture
{
    public GetTopicTest(ReadonlyIntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetTopicById_ShouldReturnTopic_WhenTopicExist(long topicId)
    {
        // Arrange
        GetTopic.Query query = new("1", topicId);

        // Act
        GetTopic.ResponseDto result = await _mediator.Send(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.TopicDto.TopicId, topicId);

        if (result.TopicDto.EventDtos.Count > 1)
        {
            // проверяет, что события расположены в порядке убывания от самого нового по дате обновления
            for (int i = 0; i < result.TopicDto.EventDtos.Count - 1; i++)
            {
                Assert.True(result.TopicDto.EventDtos[i].UpdateDate >= result.TopicDto.EventDtos[i + 1].UpdateDate,
                    $"Элемент {i} должен быть новее или равен элементу {i + 1}");
            }
        }
    }

    [Theory]
    [InlineData(6666666666)]
    [InlineData(6666666667)]
    public async Task GetTopicById_ShouldReturnTopic_WhenTopicNotExist(long topicId)
    {
        // Arrange
        GetTopic.Query query = new("1", topicId);

        // Act && Assert
        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _mediator.Send(request: query));
    }
}
