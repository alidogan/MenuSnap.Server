namespace Identity.TenantMembers.Features.CreateTenantMember;

public record CreateTenantMemberRequest(
    Guid TenantId,
    string Email,
    string DisplayName,
    string Role,
    string Password,
    bool IsActive = true);

public record CreateTenantMemberResponse(Guid Id);

public class CreateTenantMemberEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tenant-members", async (CreateTenantMemberRequest request, ISender sender) =>
        {
            var command = new CreateTenantMemberCommand(
                request.TenantId, request.Email,
                request.DisplayName, request.Role, request.Password, request.IsActive);

            var result = await sender.Send(command);
            var response = new CreateTenantMemberResponse(result.Id);
            return Results.Created($"/tenant-members/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateTenantMember")
        .Produces<CreateTenantMemberResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Tenant Member")
        .WithDescription("Add a user to a tenant with a specific role, creating the user account if needed");
    }
}
