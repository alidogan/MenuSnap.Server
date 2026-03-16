namespace ServiceUnit.ServiceUnits.Features.GetServiceUnitsByLocation;

public record GetServiceUnitsByLocationResponse(IReadOnlyList<ServiceUnitDto> ServiceUnits);

public class GetServiceUnitsByLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/locations/{locationId:guid}/service-units",
            async (Guid locationId, ISender sender) =>
            {
                var result = await sender.Send(new GetServiceUnitsByLocationQuery(locationId));
                return Results.Ok(new GetServiceUnitsByLocationResponse(result.ServiceUnits));
            })
        .RequireAuthorization()
        .WithName("GetServiceUnitsByLocation")
        .Produces<GetServiceUnitsByLocationResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Service Units by Location");
    }
}
