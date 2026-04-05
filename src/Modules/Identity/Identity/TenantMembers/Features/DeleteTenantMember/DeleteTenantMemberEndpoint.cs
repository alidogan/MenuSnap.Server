namespace Identity.TenantMembers.Features.DeleteTenantMember;

public record DeleteTenantMemberResponse(bool IsSuccess);

public class DeleteTenantMemberEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tenant-members/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteTenantMemberCommand(id));
            var response = new DeleteTenantMemberResponse(result.IsSuccess);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("DeleteTenantMember")
        .Produces<DeleteTenantMemberResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Tenant Member")
        .WithDescription("Remove a member from a tenant (soft delete)");
    }
}
