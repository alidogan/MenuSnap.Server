using Carter;
using Catalog;
using Identity;
using Keycloak.AuthServices.Authentication;
using Location;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Order;
using Serilog;
using ServiceUnit;
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
var serviceUnitAssembly = typeof(ServiceUnitModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly, serviceUnitAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly, serviceUnitAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, locationAssembly, orderAssembly, tenantAssembly, identityAssembly, serviceUnitAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);

var keycloakFrontendUrl = builder.Configuration["Keycloak:frontend-url"];
if (!string.IsNullOrEmpty(keycloakFrontendUrl))
{
    var realm = builder.Configuration["Keycloak:realm"];
    var frontendIssuer = $"{keycloakFrontendUrl.TrimEnd('/')}/realms/{realm}";
    builder.Services.PostConfigureAll<JwtBearerOptions>(options =>
    {
        var existing = (options.TokenValidationParameters.ValidIssuers ?? []).ToList();
        if (!string.IsNullOrEmpty(options.TokenValidationParameters.ValidIssuer))
            existing.Add(options.TokenValidationParameters.ValidIssuer);
        existing.Add(frontendIssuer);
        options.TokenValidationParameters.ValidIssuer = null;
        options.TokenValidationParameters.ValidIssuers = existing.Distinct();
    });
}

builder.Services.AddAuthorization();

builder.Services
    .AddStorageModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddLocationModule(builder.Configuration)
    .AddOrderModule(builder.Configuration)
    .AddTenantModule(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddServiceUnitModule(builder.Configuration);

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
    .UseIdentityModule()
    .UseServiceUnitModule();

app.Run();

public partial class Program { }
