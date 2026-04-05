namespace Catalog.Groups.Features.UpdateGroup;

public record UpdateGroupRequest(string Name, string? Description, string Type, int DisplayOrder, bool IsActive,
    Dictionary<string, LocalizedContent>? Translations = null);

public class UpdateGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/groups/{id:guid}", async (Guid id, UpdateGroupRequest request, ISender sender) =>
        {
            await sender.Send(new UpdateGroupCommand(
                id, request.Name, request.Description, request.Type, request.DisplayOrder, request.IsActive,
                request.Translations));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateCatalogGroup")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Catalog Group");
    }
}
