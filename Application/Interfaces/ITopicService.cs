using Application.Features.TopicFeatures.Command;
using Application.Features.TopicFeatures.Query;

namespace Application.Interfaces;

public interface ITopicService
{
    /// <summary>
    /// Создаёт или обновляет топик
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="topicDto">Данные топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного/обновлённого топика</returns>
    public Task<long> CreateOrUpdateTopicAsync(long userId, long? topicId, CreateOrUpdateTopic.RequestDto topicDto, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список топиков
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    public Task<GetListTopic.ResponseDto> GetListTopicAsync(long userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает топик по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Топик и связанные с ним события</returns>
    public Task<Topic> GetTopicByIdAsync(long userId, long topicId, CancellationToken cancellationToken);
}
