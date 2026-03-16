namespace Storage.Options;

public class StorageOptions
{
    public const string SectionName = "Storage";

    public string Endpoint { get; set; } = "localhost:9000";
    /// <summary>
    /// Public-facing endpoint used to build object URLs returned to clients.
    /// Defaults to Endpoint when not set.
    /// </summary>
    public string? PublicEndpoint { get; set; }
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string BucketName { get; set; } = "menusnap";
    public bool UseSSL { get; set; } = false;
}
