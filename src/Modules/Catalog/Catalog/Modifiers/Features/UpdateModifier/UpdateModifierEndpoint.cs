namespace Catalog.Modifiers.Features.UpdateModifier;

public record UpdateModifierRequest(string Name, decimal PriceDelta, bool IsDefault, bool IsAvailable, int DisplayOrder);

public class UpdateModifierEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/modifier-groups/{groupId:guid}/modifiers/{id:guid}", async (
                Guid groupId, Guid id, UpdateModifierRequest request, ISender sender) =>
            {
                await sender.Send(new UpdateModifierCommand(
                    id, request.Name, request.PriceDelta,
                    request.IsDefault, request.IsAvailable, request.DisplayOrder));
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("UpdateItemModifier")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Item Modifier");
    }
}
