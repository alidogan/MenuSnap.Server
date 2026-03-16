namespace Location.Locations.Features.UpdateLocation;

public record UpdateLocationRequest(
    string Name,
    string Slug,
    string Type,
    string? Address,
    string? Phone,
    string? Description,
    bool IsActive);

public class UpdateLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/locations/{id:guid}", async (Guid id, UpdateLocationRequest request, ISender sender) =>
        {
            var command = new UpdateLocationCommand(
                id, request.Name, request.Slug, request.Type,
                request.Address, request.Phone, request.Description, request.IsActive);

            await sender.Send(command);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateLocation")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Location");
    }
}
