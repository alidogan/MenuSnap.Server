using Identity.Contracts.TenantMembers.Dtos;

namespace Identity.TenantMembers.Features.GetTenantMembers;

public record GetTenantMembersResponse(IEnumerable<TenantMemberDto> Members);

public class GetTenantMembersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tenant-members", async (
            [AsParameters] GetTenantMembersQuery query,
            ISender sender) =>
        {
            var result = await sender.Send(query);
            var response = new GetTenantMembersResponse(result.Members);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetTenantMembers")
        .Produces<GetTenantMembersResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Tenant Members")
        .WithDescription("Get all members of a tenant");
    }
}
