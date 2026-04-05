namespace Identity.TenantMembers.Features.UpdateTenantMember;

public record UpdateTenantMemberRequest(
    Guid Id,
    string DisplayName,
    string Role,
    bool IsActive);

public record UpdateTenantMemberResponse(bool IsSuccess);

public class UpdateTenantMemberEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tenant-members", async (UpdateTenantMemberRequest request, ISender sender) =>
        {
            var command = new UpdateTenantMemberCommand(
                request.Id, request.DisplayName, request.Role, request.IsActive);

            var result = await sender.Send(command);
            var response = new UpdateTenantMemberResponse(result.IsSuccess);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("UpdateTenantMember")
        .Produces<UpdateTenantMemberResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Tenant Member")
        .WithDescription("Update the role or status of a tenant member");
    }
}
