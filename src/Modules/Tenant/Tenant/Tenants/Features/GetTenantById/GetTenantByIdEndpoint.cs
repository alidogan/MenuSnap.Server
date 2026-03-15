using Tenant.Contracts.Tenants.Features.GetTenantById;

namespace Tenant.Tenants.Features.GetTenantById;

public record GetTenantByIdResponse(TenantDto Tenant);

public class GetTenantByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tenants/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetTenantByIdQuery(id);
            var result = await sender.Send(query);
            var response = new GetTenantByIdResponse(result.Tenant);
            return Results.Ok(response);
        })
        .WithName("GetTenantById")
        .Produces<GetTenantByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Tenant By Id")
        .WithDescription("Get a tenant by its unique identifier");
    }
}
