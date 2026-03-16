namespace Catalog.Items.Features.DeleteItem;

public class DeleteItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/catalog/items/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteItemCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteCatalogItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Catalog Item");
    }
}
