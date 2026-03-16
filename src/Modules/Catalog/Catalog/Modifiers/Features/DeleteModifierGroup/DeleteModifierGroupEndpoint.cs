namespace Catalog.Modifiers.Features.DeleteModifierGroup;

public class DeleteModifierGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/catalog/modifier-groups/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteModifierGroupCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteItemModifierGroup")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Item Modifier Group");
    }
}
