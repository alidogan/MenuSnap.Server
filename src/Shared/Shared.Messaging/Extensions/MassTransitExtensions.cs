using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);
            config.AddSagaStateMachines(assemblies);
            config.AddSagas(assemblies);
            config.AddActivities(assemblies);

            config.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
