namespace ServiceUnit.ServiceUnits.Features.DeleteServiceUnit;

public class DeleteServiceUnitEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/service-units/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteServiceUnitCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteServiceUnit")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Service Unit")
        .WithDescription("Soft-deletes a service unit. The record is retained in the database.");
    }
}
