namespace Catalog.Groups.Features.DeleteGroup;

public class DeleteGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/catalog/groups/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteGroupCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteCatalogGroup")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Catalog Group");
    }
}
