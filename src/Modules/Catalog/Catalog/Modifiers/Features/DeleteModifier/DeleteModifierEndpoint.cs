namespace Catalog.Modifiers.Features.DeleteModifier;

public class DeleteModifierEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/catalog/modifier-groups/{groupId:guid}/modifiers/{id:guid}", async (
                Guid groupId, Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteModifierCommand(id));
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteItemModifier")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Item Modifier");
    }
}
