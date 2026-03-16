namespace Location.Locations.Features.GetLocationsByTenant;

public class GetLocationsByTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/locations", async ([AsParameters] GetLocationsByTenantQueryParams p, ISender sender) =>
        {
            var result = await sender.Send(new GetLocationsByTenantQuery(p.TenantId));
            return Results.Ok(new { locations = result.Locations });
        })
        .RequireAuthorization()
        .WithName("GetLocationsByTenant")
        .Produces<GetLocationsByTenantResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Locations by Tenant");
    }
}

public record GetLocationsByTenantQueryParams(Guid TenantId);
