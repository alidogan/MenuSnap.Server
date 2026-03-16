namespace Catalog.Categories.Features.DeleteCategory;

public class DeleteCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/catalog/categories/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteCategoryCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteCatalogCategory")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Catalog Category");
    }
}
