namespace Catalog.Modifiers.Features.UpdateModifierGroup;

public record UpdateModifierGroupRequest(
    string Name, bool IsRequired, bool IsMultiSelect,
    int? MinSelections, int? MaxSelections, int DisplayOrder, bool IsActive);

public class UpdateModifierGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/modifier-groups/{id:guid}", async (Guid id, UpdateModifierGroupRequest request, ISender sender) =>
        {
            await sender.Send(new UpdateModifierGroupCommand(
                id, request.Name, request.IsRequired, request.IsMultiSelect,
                request.MinSelections, request.MaxSelections, request.DisplayOrder, request.IsActive));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateItemModifierGroup")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Item Modifier Group");
    }
}
