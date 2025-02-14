using Application.Features.TopicFeatures.Query;

namespace Gryphon.IntegrationTests.TopicFeaturesTests;

public class GetListTopicTest : BaseReadonlyClassFixture
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
    }
}
