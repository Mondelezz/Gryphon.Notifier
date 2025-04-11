using Application.Extensions;
using Application.Interfaces;
using Application.Options;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace Application.Services;

public class FileDataService(
    IOptions<S3Options> s3options,
    IMinioClient minioClient,
    IFileDataRepository fileDataRepository,
    ILogger<FileDataService> logger) : IFileDataService
{
    private readonly S3Options _s3options = s3options.Value;

    public async Task<(MemoryStream destination, string? mimeType, string fileName)> DownloadAsync(long fileId, CancellationToken cancellationToken)
    {
        try
        {
            FileData fileData = await fileDataRepository.GetFileDataByIdAsync(fileId, cancellationToken)
                ?? throw new EntityNotFoundException(fileId, "FileData");

            string relativePath = GetStoragePath(fileData.FileHash);

            Minio.DataModel.ObjectStat objStatReply = await minioClient.StatObjectAsync(new StatObjectArgs()
                       .WithBucket(_s3options.BucketName)
                       .WithObject(relativePath), cancellationToken);

            if (objStatReply?.DeleteMarker != false)
            {
                throw new EntityNotFoundException($"The object '{relativePath}' was not found or deleted");
            }

            MemoryStream destination = new();

            await minioClient.GetObjectAsync(new GetObjectArgs()
                       .WithBucket(_s3options.BucketName)
                       .WithObject(relativePath)
                       .WithCallbackStream((stream) => stream.CopyTo(destination)), cancellationToken);

            destination.Position = 0;
            return (destination, mimeType: fileData.MimeType, fileName: fileData.FileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while receiving a file from S3 storage:");
            throw;
        }
    }

    public async Task<long> UploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        string tempFilePath = Path.GetTempFileName();
        try
        {
            await using (FileStream stream = new(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            string fileHash = CoreUtils.CalculateFileHash(tempFilePath);

            string relativePath = GetStoragePath(fileHash);

            await using (FileStream inputStream = new(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PutObjectArgs putObjectArgs = new PutObjectArgs()
                    .WithBucket(_s3options.BucketName)
                    .WithObject(relativePath)
                    .WithStreamData(inputStream)
                    .WithObjectSize(inputStream.Length)
                    .WithContentType(file.ContentType);

                PutObjectResponse putObject = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                if (string.IsNullOrEmpty(putObject.Etag))
                {
                    throw new InvalidOperationException("ETag is missing in the response from MinIO");
                }

                await VerifiesFileIntegrityAsync(putObject, fileHash, relativePath, cancellationToken);
            }

            FileData fileData = new()
            {
                FileHash = fileHash,
                FileName = file.FileName,
                MimeType = file.ContentType,
                CreateDate = DateTime.UtcNow,
            };

            await fileDataRepository.AddDataAsync(fileData, cancellationToken);

            return fileData.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when uploading a file to S3 or saving it to a database.");
            throw;
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    /// <summary>
    /// Вычисляет относительный путь к файлу
    /// </summary>
    /// <param name="fileHash">Хеш файла</param>
    /// <returns>Относительный путь</returns>
    private string GetStoragePath(string fileHash) =>
        string.IsNullOrEmpty(_s3options.StorageDataFilesPath)
            ? fileHash
            : Path.Combine(_s3options.StorageDataFilesPath, fileHash);

    /// <summary>
    /// Проверяет целостность файла
    /// </summary>
    /// <param name="putObject">Созданный объект</param>
    /// <param name="fileHash">Хеш файла</param>
    /// <param name="relativePath">Относительный путь до файла в хранилище</param>
    /// <param name="cancellationToken"><Токен отмены/param>
    private async Task VerifiesFileIntegrityAsync(
        PutObjectResponse putObject,
        string fileHash,
        string relativePath,
        CancellationToken cancellationToken)
    {
        // Проверка файла на целостность после загрузки
        string etag = putObject.Etag.Replace("\"", "").Replace("\\", "");

        if (!etag.Equals(fileHash, StringComparison.Ordinal))
        {
            // Удаление файла из хранилища, если не прошел проверку
            RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_s3options.BucketName)
                .WithObject(relativePath);

            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            throw new InvalidOperationException("Error uploading the file: File integrity check failed. Hashes do not match.");
        }
    }
}
