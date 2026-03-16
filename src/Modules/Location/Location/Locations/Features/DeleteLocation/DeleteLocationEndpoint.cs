namespace Location.Locations.Features.DeleteLocation;

public class DeleteLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/locations/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteLocationCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteLocation")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Location");
    }
}
