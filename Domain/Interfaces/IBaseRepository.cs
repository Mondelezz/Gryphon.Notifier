namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Добавляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленная сущность</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет сущность
    /// </summary>
    /// <param name="modifierEntity">обновляемая сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность</returns>
    Task<T> UpdateAsync(T modifierEntity, CancellationToken cancellationToken);

    /// <summary>
    /// Получает сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность</returns>
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Запросить все сущности из базы данных
    /// </summary>
    /// <returns>IQueryable массив сущностей</returns>
    IQueryable<T> GetAll();
}
