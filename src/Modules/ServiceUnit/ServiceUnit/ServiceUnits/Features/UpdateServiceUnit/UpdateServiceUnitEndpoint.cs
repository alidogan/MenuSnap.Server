namespace ServiceUnit.ServiceUnits.Features.UpdateServiceUnit;

public record UpdateServiceUnitRequest(
    string Name,
    string Code,
    string Type,
    int? Capacity,
    string? GroupName,
    string? ExternalReference,
    int SortOrder,
    string Status,
    bool IsActive);

public class UpdateServiceUnitEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/service-units/{id:guid}", async (Guid id, UpdateServiceUnitRequest request, ISender sender) =>
        {
            var command = new UpdateServiceUnitCommand(
                id, request.Name, request.Code, request.Type,
                request.Capacity, request.GroupName, request.ExternalReference,
                request.SortOrder, request.Status, request.IsActive);

            await sender.Send(command);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateServiceUnit")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Service Unit");
    }
}
