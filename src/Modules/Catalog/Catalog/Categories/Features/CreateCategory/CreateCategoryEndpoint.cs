namespace Catalog.Categories.Features.CreateCategory;

public record CreateCategoryRequest(Guid TenantId, Guid GroupId, string Name, string? Description, int DisplayOrder, bool IsActive = true,
    Dictionary<string, LocalizedContent>? Translations = null);
public record CreateCategoryResponse(Guid Id);

public class CreateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/catalog/categories", async (CreateCategoryRequest request, ISender sender) =>
        {
            var command = new CreateCategoryCommand(
                request.TenantId, request.GroupId, request.Name,
                request.Description, request.DisplayOrder, request.IsActive,
                request.Translations);
            var result = await sender.Send(command);
            return Results.Created($"/catalog/categories/{result.Id}", new CreateCategoryResponse(result.Id));
        })
        .RequireAuthorization()
        .WithName("CreateCatalogCategory")
        .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Catalog Category");
    }
}
