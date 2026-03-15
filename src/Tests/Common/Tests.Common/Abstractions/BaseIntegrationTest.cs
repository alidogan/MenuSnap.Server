using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Collections;
using Tests.Common.Factories;
using Xunit;

namespace Tests.Common.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private readonly IServiceScope _scope;

    protected readonly HttpClient Client;
    protected readonly IServiceProvider Services;

    protected BaseIntegrationTest(MenuSnapWebAppFactory factory)
    {
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        Services = _scope.ServiceProvider;
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync()
    {
        _scope.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }
}
