using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileDataService
{
    /// <summary>
    /// Скачивает файл из хранилища
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл из хранилища</returns>
    Task<(MemoryStream destination, string? mimeType, string fileName)> DownloadAsync(long fileId, CancellationToken cancellationToken);

    /// <summary>
    /// Загружает файл в хранилище
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор, по которому в дальнейшем можно будет получить файл</returns>
    Task<long> UploadAsync(IFormFile file, CancellationToken cancellationToken);
}
