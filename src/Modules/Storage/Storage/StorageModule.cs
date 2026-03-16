using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Storage.Contracts;
using Storage.Options;
using Storage.Services;

namespace Storage;

public static class StorageModule
{
    public static IServiceCollection AddStorageModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var opts = configuration
            .GetSection(StorageOptions.SectionName)
            .Get<StorageOptions>() ?? new StorageOptions();

        services.Configure<StorageOptions>(
            configuration.GetSection(StorageOptions.SectionName));

        services.AddMinio(configureClient => configureClient
            .WithEndpoint(opts.Endpoint)
            .WithCredentials(opts.AccessKey, opts.SecretKey)
            .WithSSL(opts.UseSSL)
            .Build());

        services.AddScoped<IStorageService, MinioStorageService>();

        return services;
    }

    public static IApplicationBuilder UseStorageModule(this IApplicationBuilder app) => app;
}
