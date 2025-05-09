using Application.Features.TopicFeatures.Query;

namespace Application.Services;

public partial class TopicService
{
    /// <summary>
    /// Получает список топиков
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    public async Task<GetListTopic.ResponseDto> GetListTopicAsync(long userId, CancellationToken cancellationToken)
    {
        IReadOnlyList<Topic> topics = await topicRepository
            .GetAll()
            .Where(t => t.UserId == userId && !t.IsDeleted)
            .OrderByDescending(t => t.UpdateDate)
            .ToListAsync(cancellationToken);

        int totalCount = topics.Count;

        if (totalCount == 0)
        {
            return new GetListTopic.ResponseDto([], totalCount);
        }

        return new GetListTopic.ResponseDto(GetListTopic.Mapper.Map(topics), totalCount);
    }
}
