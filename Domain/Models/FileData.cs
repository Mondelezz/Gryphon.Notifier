using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Метаданные файла
/// </summary>
public class FileData : EntityBase
{
    /// <summary>
    /// Хеш файла
    /// </summary>
    public required string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// Наименование файла
    /// </summary>
    public required string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Тип Mime, описывает содержимое файла
    /// </summary>
    public string? MimeType { get; set; }
}
