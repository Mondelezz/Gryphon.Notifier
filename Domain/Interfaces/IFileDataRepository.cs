using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces;

public interface IFileDataRepository : IBaseRepository<FileData>
{
    /// <summary>
    /// Добавляет и сохраняет в базе данных сущность FileData
    /// </summary>
    /// <param name="fileData">Данные файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленный объект</returns>
    Task<FileData> AddDataAsync(FileData fileData, CancellationToken cancellationToken);

    /// <summary>
    /// Получает сущность FileData по идентификатору
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные файла</returns>
    Task<FileData?> GetFileDataByIdAsync(long fileId, CancellationToken cancellationToken);
}
