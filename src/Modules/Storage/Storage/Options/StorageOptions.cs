namespace Storage.Options;

public class StorageOptions
{
    public const string SectionName = "Storage";

    public string Endpoint { get; set; } = "localhost:9000";
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string BucketName { get; set; } = "menusnap";
    public bool UseSSL { get; set; } = false;
}
