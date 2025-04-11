using System.Security.Cryptography;

namespace Application.Extensions;

public static class CoreUtils
{
    /// <summary>
    /// Вычисляет хеш файла
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <returns>Хеш</returns>
    public static string CalculateFileHash(string filePath)
    {
        using (var md5 = MD5.Create())
        using (var stream = File.OpenRead(filePath))
        {
            byte[] hashBytes = md5.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
