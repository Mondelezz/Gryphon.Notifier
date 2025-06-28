using Domain.Models;

namespace Domain.Interfaces;

public interface ITopicRepository : IBaseRepository<Topic>
{
    /// <summary>
    /// Добавляет и сохраняет в базе данных сущность Topic
    /// </summary>
    /// <param name="topic">Топик</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленный объект</returns>
    Task<Topic> AddTopicAsync(Topic topic, CancellationToken cancellationToken);

    /// <summary>
    /// Получает сущность Topic по идентификатору
    /// </summary>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные топика</returns>
    Task<Topic?> GetTopicByIdAsync(long topicId, CancellationToken cancellationToken);
}
