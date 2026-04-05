namespace Catalog.Modifiers.Features.CreateModifierGroup;

public record CreateModifierGroupRequest(
    Guid TenantId, Guid ItemId, string Name,
    bool IsRequired, bool IsMultiSelect,
    int? MinSelections, int? MaxSelections,
    int DisplayOrder, bool IsActive = true,
    Dictionary<string, LocalizedContent>? Translations = null);

public record CreateModifierGroupResponse(Guid Id);

public class CreateModifierGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/catalog/modifier-groups", async (CreateModifierGroupRequest request, ISender sender) =>
        {
            var command = new CreateModifierGroupCommand(
                request.TenantId, request.ItemId, request.Name,
                request.IsRequired, request.IsMultiSelect,
                request.MinSelections, request.MaxSelections,
                request.DisplayOrder, request.IsActive,
                request.Translations);

            var result = await sender.Send(command);
            return Results.Created($"/catalog/modifier-groups/{result.Id}", new CreateModifierGroupResponse(result.Id));
        })
        .RequireAuthorization()
        .WithName("CreateItemModifierGroup")
        .Produces<CreateModifierGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Item Modifier Group");
    }
}
