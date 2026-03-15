namespace Tenant.Tenants.Features.CreateTenant;

public record CreateTenantRequest(string Name, string Slug, string? LogoUrl, bool IsActive);
public record CreateTenantResponse(Guid Id);

public class CreateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tenants", async (CreateTenantRequest request, ISender sender) =>
        {
            var command = new CreateTenantCommand(request.Name, request.Slug, request.LogoUrl, request.IsActive);
            var result = await sender.Send(command);
            var response = new CreateTenantResponse(result.Id);
            return Results.Created($"/tenants/{response.Id}", response);
        })
        .WithName("CreateTenant")
        .Produces<CreateTenantResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Tenant")
        .WithDescription("Create a new tenant");
    }
}
