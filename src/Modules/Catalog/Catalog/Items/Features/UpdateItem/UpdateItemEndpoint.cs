namespace Catalog.Items.Features.UpdateItem;

public record UpdateItemRequest(
    string Name, string? Description, decimal Price,
    int? Calories, int? PrepTimeMinutes, bool IsAvailable,
    int DisplayOrder, List<string>? Allergens, List<string>? Badges,
    Dictionary<string, LocalizedContent>? Translations = null);

public class UpdateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/items/{id:guid}", async (Guid id, UpdateItemRequest request, ISender sender) =>
        {
            await sender.Send(new UpdateItemCommand(
                id, request.Name, request.Description, request.Price,
                request.Calories, request.PrepTimeMinutes, request.IsAvailable,
                request.DisplayOrder, request.Allergens, request.Badges, request.Translations));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateCatalogItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Catalog Item");
    }
}
