using Application.Features.TopicFeatures.Query;


namespace Gryphon.IntegrationTests.TopicFeaturesTests;

public sealed class GetListTopicTest : BaseReadonlyClassFixture
{
    public GetListTopicTest(ReadonlyIntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetListTopic_ReturnCorrectResult()
    {
        // Arrange
        GetListTopic.Query query = new(
            CurrentUserId: "1");

        // Act
        GetListTopic.ResponseDto result = await _mediator.Send(query);

        // Assert
        Assert.Equal(2, result.TotalCount);

        for (int i = 0; i < result.TopicDtos.Count - 1; i++)
        {
            Assert.True(result.TopicDtos[i].UpdateDate >= result.TopicDtos[i + 1].UpdateDate, $"Элемент {i} должен быть новее или равен элементу {i + 1}");
        }
    }
}
