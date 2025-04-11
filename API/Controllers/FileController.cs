using Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/files")]
[ApiController]
public class FileController(IFileDataService fileDataService) : ControllerBase
{
    /// <summary>
    /// Загружает файл в хранилище
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор загруженного файла</returns>
    [HttpPost]
    public async Task<ActionResult<long>> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        long fileId = await fileDataService.UploadAsync(file, cancellationToken);

        if (fileId == 0)
        {
            return BadRequest();
        }

        return Ok(fileId);
    }

    /// <summary>
    /// Скачивает файл из хранилища
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл из хранилища</returns>
    [HttpGet("{fileId}")]
    public async Task<ActionResult> DownloadAsync(long fileId, CancellationToken cancellationToken = default)
    {
        (MemoryStream stream, string? mimeType, string fileName) = await fileDataService.DownloadAsync(fileId, cancellationToken);

        return new FileStreamResult(stream, mimeType ?? "")
        {
            FileDownloadName = fileName
        };
    }
}
