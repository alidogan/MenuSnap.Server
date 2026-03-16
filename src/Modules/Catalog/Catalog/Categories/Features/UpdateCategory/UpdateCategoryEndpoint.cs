namespace Catalog.Categories.Features.UpdateCategory;

public record UpdateCategoryRequest(string Name, string? Description, int DisplayOrder, bool IsActive);

public class UpdateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/categories/{id:guid}", async (Guid id, UpdateCategoryRequest request, ISender sender) =>
        {
            await sender.Send(new UpdateCategoryCommand(id, request.Name, request.Description, request.DisplayOrder, request.IsActive));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateCatalogCategory")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Catalog Category");
    }
}
