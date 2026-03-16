using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Storage.Contracts;
using Storage.Options;

namespace Storage.Services;

internal sealed class MinioStorageService(
    IMinioClient minio,
    IOptions<StorageOptions> opts,
    ILogger<MinioStorageService> logger)
    : IStorageService
{
    private readonly StorageOptions _opts = opts.Value;

    public async Task<string> UploadAsync(
        string module,
        string tenantId,
        string key,
        Stream content,
        string contentType,
        CancellationToken ct = default)
    {
        await EnsureBucketExistsAsync(ct);

        var objectName = $"{module}/{tenantId}/{key}";
        var size = content.CanSeek ? content.Length : -1;

        var putArgs = new PutObjectArgs()
            .WithBucket(_opts.BucketName)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await minio.PutObjectAsync(putArgs, ct);

        var protocol = _opts.UseSSL ? "https" : "http";
        var publicHost = string.IsNullOrWhiteSpace(_opts.PublicEndpoint) ? _opts.Endpoint : _opts.PublicEndpoint;
        var url = $"{protocol}://{publicHost}/{_opts.BucketName}/{objectName}";

        logger.LogInformation("Uploaded object {ObjectName} to bucket {Bucket}", objectName, _opts.BucketName);

        return url;
    }

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_opts.BucketName)
            .WithObject(path);

        await minio.RemoveObjectAsync(removeArgs, ct);

        logger.LogInformation("Deleted object {Path} from bucket {Bucket}", path, _opts.BucketName);
    }

    public async Task<string> GetPresignedUrlAsync(
        string path,
        int expirySeconds = 3600,
        CancellationToken ct = default)
    {
        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(_opts.BucketName)
            .WithObject(path)
            .WithExpiry(expirySeconds);

        return await minio.PresignedGetObjectAsync(presignedArgs);
    }

    private async Task EnsureBucketExistsAsync(CancellationToken ct)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_opts.BucketName);
        var exists = await minio.BucketExistsAsync(bucketExistsArgs, ct);

        if (!exists)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_opts.BucketName);
            await minio.MakeBucketAsync(makeBucketArgs, ct);
            logger.LogInformation("Created MinIO bucket {Bucket}", _opts.BucketName);
        }

        await SetPublicReadPolicyAsync(ct);
    }

    private async Task SetPublicReadPolicyAsync(CancellationToken ct)
    {
        var policy = $$"""
            {
              "Version": "2012-10-17",
              "Statement": [
                {
                  "Effect": "Allow",
                  "Principal": { "AWS": ["*"] },
                  "Action": ["s3:GetObject"],
                  "Resource": ["arn:aws:s3:::{{_opts.BucketName}}/*"]
                }
              ]
            }
            """;

        var setPolicyArgs = new SetPolicyArgs()
            .WithBucket(_opts.BucketName)
            .WithPolicy(policy);

        await minio.SetPolicyAsync(setPolicyArgs, ct);
    }
}
