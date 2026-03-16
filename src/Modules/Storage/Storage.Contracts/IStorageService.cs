namespace Storage.Contracts;

public interface IStorageService
{
    /// <summary>
    /// Uploads a file and returns the public URL.
    /// Key path: {module}/{tenantId}/{key}  e.g. catalog/abc/items/7f3a/photo.jpg
    /// </summary>
    Task<string> UploadAsync(
        string module,
        string tenantId,
        string key,
        Stream content,
        string contentType,
        CancellationToken ct = default);

    Task DeleteAsync(string path, CancellationToken ct = default);

    Task<string> GetPresignedUrlAsync(
        string path,
        int expirySeconds = 3600,
        CancellationToken ct = default);
}
