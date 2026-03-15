namespace Tenant.Tenants.Features.GetTenants;

public record GetTenantsResponse(PaginatedResult<TenantDto> Tenants);

public class GetTenantsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tenants", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var query = new GetTenantsQuery(request);
            var result = await sender.Send(query);
            var response = new GetTenantsResponse(result.Tenants);
            return Results.Ok(response);
        })
        .WithName("GetTenants")
        .Produces<GetTenantsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Tenants")
        .WithDescription("Get a paginated list of tenants");
    }
}
