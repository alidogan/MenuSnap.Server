using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace ServiceUnit;

public static class ServiceUnitModule
{
    public static IServiceCollection AddServiceUnitModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<Data.ServiceUnitDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, Data.Seed.ServiceUnitDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseServiceUnitModule(this IApplicationBuilder app)
    {
        app.UseMigration<Data.ServiceUnitDbContext>();
        return app;
    }
}
