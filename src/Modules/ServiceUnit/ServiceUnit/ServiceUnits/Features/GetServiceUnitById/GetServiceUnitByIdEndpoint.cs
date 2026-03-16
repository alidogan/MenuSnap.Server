namespace ServiceUnit.ServiceUnits.Features.GetServiceUnitById;

public record GetServiceUnitByIdResponse(ServiceUnitDto ServiceUnit);

public class GetServiceUnitByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/service-units/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetServiceUnitByIdQuery(id));
            return Results.Ok(new GetServiceUnitByIdResponse(result.ServiceUnit));
        })
        .RequireAuthorization()
        .WithName("GetServiceUnitById")
        .Produces<GetServiceUnitByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Service Unit by ID");
    }
}
