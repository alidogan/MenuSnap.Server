using Carter;
using Catalog;
using Identity;
using Keycloak.AuthServices.Authentication;
using Location;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Order;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;
using Shared.Messaging.Extensions;
using Storage;
using Tenant;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var catalogAssembly = typeof(CatalogModule).Assembly;
var locationAssembly = typeof(LocationModule).Assembly;
var orderAssembly = typeof(OrderModule).Assembly;
var tenantAssembly = typeof(TenantModule).Assembly;
var identityAssembly = typeof(IdentityModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddStorageModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddLocationModule(builder.Configuration)
    .AddOrderModule(builder.Configuration)
    .AddTenantModule(builder.Configuration)
    .AddIdentityModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app
    .UseCatalogModule()
    .UseLocationModule()
    .UseOrderModule()
    .UseTenantModule()
    .UseIdentityModule();

app.Run();

public partial class Program { }
