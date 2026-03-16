namespace ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

public record CreateServiceUnitRequest(
    Guid LocationId,
    string Name,
    string Code,
    string Type,
    int? Capacity,
    string? GroupName,
    string? ExternalReference,
    int SortOrder,
    string? Status,
    bool IsActive = true);

public record CreateServiceUnitResponse(Guid Id);

public class CreateServiceUnitEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/service-units", async (CreateServiceUnitRequest request, ISender sender) =>
        {
            var command = new CreateServiceUnitCommand(
                request.LocationId, request.Name, request.Code, request.Type,
                request.Capacity, request.GroupName, request.ExternalReference,
                request.SortOrder, request.Status, request.IsActive);

            var result = await sender.Send(command);
            var response = new CreateServiceUnitResponse(result.Id);
            return Results.Created($"/service-units/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateServiceUnit")
        .Produces<CreateServiceUnitResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Service Unit")
        .WithDescription("Create a new service unit (QR code placement) for a location");
    }
}
