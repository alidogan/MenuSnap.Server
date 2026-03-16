namespace Catalog.Groups.Features.CreateGroup;

public record CreateGroupRequest(
    Guid TenantId,
    Guid LocationId,
    string Name,
    string? Description,
    string Type,
    int DisplayOrder,
    bool IsActive = true);

public record CreateGroupResponse(Guid Id);

public class CreateGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/catalog/groups", async (CreateGroupRequest request, ISender sender) =>
        {
            var command = new CreateGroupCommand(
                request.TenantId, request.LocationId, request.Name,
                request.Description, request.Type, request.DisplayOrder, request.IsActive);

            var result = await sender.Send(command);
            return Results.Created($"/catalog/groups/{result.Id}", new CreateGroupResponse(result.Id));
        })
        .RequireAuthorization()
        .WithName("CreateCatalogGroup")
        .Produces<CreateGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Catalog Group");
    }
}
