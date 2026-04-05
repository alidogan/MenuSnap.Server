namespace Catalog.Items.Features.CreateItem;

public record CreateItemRequest(
    Guid TenantId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int? Calories,
    int? PrepTimeMinutes,
    bool IsAvailable,
    int DisplayOrder,
    List<string>? Allergens,
    List<string>? Badges,
    Dictionary<string, LocalizedContent>? Translations = null);

public record CreateItemResponse(Guid Id);

public class CreateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/catalog/items", async (CreateItemRequest request, ISender sender) =>
        {
            var command = new CreateItemCommand(
                request.TenantId, request.CategoryId, request.Name,
                request.Description, request.Price, request.Calories,
                request.PrepTimeMinutes, request.IsAvailable, request.DisplayOrder,
                request.Allergens, request.Badges, request.Translations);

            var result = await sender.Send(command);
            return Results.Created($"/catalog/items/{result.Id}", new CreateItemResponse(result.Id));
        })
        .RequireAuthorization()
        .WithName("CreateCatalogItem")
        .Produces<CreateItemResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Catalog Item");
    }
}
