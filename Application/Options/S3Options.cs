namespace Application.Options;

public class S3Options
{
    public string StorageDataFilesPath { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public string AccessKey { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public string BucketName { get; set; } = string.Empty;
}
