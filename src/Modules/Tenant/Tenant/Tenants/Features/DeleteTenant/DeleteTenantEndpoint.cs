namespace Tenant.Tenants.Features.DeleteTenant;

public record DeleteTenantResponse(bool IsSuccess);

public class DeleteTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tenants/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteTenantCommand(id);
            var result = await sender.Send(command);
            var response = new DeleteTenantResponse(result.IsSuccess);
            return Results.Ok(response);
        })
        .WithName("DeleteTenant")
        .Produces<DeleteTenantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Tenant")
        .WithDescription("Delete an existing tenant");
    }
}
