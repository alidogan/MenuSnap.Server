using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Tests.Common.Auth;
using Xunit;

namespace Tests.Common.Factories;

public class MenuSnapWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Database", _dbContainer.GetConnectionString());

        builder.ConfigureServices(services =>
        {
            RemoveMassTransitServices(services);
            services.AddMassTransit(cfg => cfg.UsingInMemory((ctx, inMemoryCfg) =>
                inMemoryCfg.ConfigureEndpoints(ctx)));

            RemoveRedisServices(services);
            services.AddDistributedMemoryCache();

            services.RemoveAll<IAuthenticationSchemeProvider>();
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, _ => { });
        });
    }

    private static void RemoveMassTransitServices(IServiceCollection services)
    {
        var massTransitDescriptors = services
            .Where(d => d.ServiceType.Namespace?.StartsWith("MassTransit") == true)
            .ToList();

        foreach (var descriptor in massTransitDescriptors)
            services.Remove(descriptor);
    }

    private static void RemoveRedisServices(IServiceCollection services)
    {
        var redisDescriptors = services
            .Where(d => d.ServiceType == typeof(RedisCacheOptions) ||
                        d.ImplementationType?.Name.Contains("Redis") == true ||
                        d.ImplementationType?.Name.Contains("StackExchange") == true)
            .ToList();

        foreach (var descriptor in redisDescriptors)
            services.Remove(descriptor);
    }
}
