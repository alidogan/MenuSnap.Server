namespace Tenant.Tenants.Features.UpdateTenant;

public record UpdateTenantRequest(Guid Id, string Name, string Slug, string? LogoUrl, bool IsActive);
public record UpdateTenantResponse(bool IsSuccess);

public class UpdateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tenants", async (UpdateTenantRequest request, ISender sender) =>
        {
            var command = new UpdateTenantCommand(request.Id, request.Name, request.Slug, request.LogoUrl, request.IsActive);
            var result = await sender.Send(command);
            var response = new UpdateTenantResponse(result.IsSuccess);
            return Results.Ok(response);
        })
        .WithName("UpdateTenant")
        .Produces<UpdateTenantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Tenant")
        .WithDescription("Update an existing tenant");
    }
}
