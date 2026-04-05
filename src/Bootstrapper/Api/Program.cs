using System.Text;
using Carter;
using Catalog;
using Identity;
using Location;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddStorageModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddLocationModule(builder.Configuration)
    .AddOrderModule(builder.Configuration)
    .AddTenantModule(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddServiceUnitModule(builder.Configuration);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "MenuSnap API",
            Version = "v1",
        };
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

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

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MenuSnap API");
    options.RoutePrefix = "swagger";
});

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

internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider schemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var schemes = await schemeProvider.GetAllSchemesAsync();
        if (!schemes.Any(s => s.Name == JwtBearerDefaults.AuthenticationScheme)) return;

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        };
    }
}
