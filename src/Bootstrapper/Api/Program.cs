using Carter;
using Identity;
using Keycloak.AuthServices.Authentication;
using Menu;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Order;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;
using Shared.Messaging.Extensions;
using Tenant;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var menuAssembly = typeof(MenuModule).Assembly;
var orderAssembly = typeof(OrderModule).Assembly;
var tenantAssembly = typeof(TenantModule).Assembly;
var identityAssembly = typeof(IdentityModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(menuAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services
    .AddMediatRWithAssemblies(menuAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, menuAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddMenuModule(builder.Configuration)
    .AddOrderModule(builder.Configuration)
    .AddTenantModule(builder.Configuration)
    .AddIdentityModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

app
    .UseMenuModule()
    .UseOrderModule()
    .UseTenantModule()
    .UseIdentityModule();

app.Run();

public partial class Program { }
