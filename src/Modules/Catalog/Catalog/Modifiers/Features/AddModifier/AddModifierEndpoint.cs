namespace Catalog.Modifiers.Features.AddModifier;

public record AddModifierRequest(string Name, decimal PriceDelta, bool IsDefault, bool IsAvailable, int DisplayOrder,
    Dictionary<string, LocalizedContent>? Translations = null);
public record AddModifierResponse(Guid Id);

public class AddModifierEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/catalog/modifier-groups/{groupId:guid}/modifiers", async (
                Guid groupId, AddModifierRequest request, ISender sender) =>
            {
                var command = new AddModifierCommand(
                    groupId, request.Name, request.PriceDelta,
                    request.IsDefault, request.IsAvailable, request.DisplayOrder,
                    request.Translations);

                var result = await sender.Send(command);
                return Results.Created(
                    $"/catalog/modifier-groups/{groupId}/modifiers/{result.Id}",
                    new AddModifierResponse(result.Id));
            })
            .RequireAuthorization()
            .WithName("AddItemModifier")
            .Produces<AddModifierResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Add Item Modifier");
    }
}
