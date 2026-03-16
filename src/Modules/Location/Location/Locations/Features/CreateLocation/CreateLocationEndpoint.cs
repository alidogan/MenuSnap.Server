namespace Location.Locations.Features.CreateLocation;

public record CreateLocationRequest(
    Guid TenantId,
    string Name,
    string Slug,
    string Type,
    string? Address,
    string? Phone,
    string? Description,
    bool IsActive = true);

public record CreateLocationResponse(Guid Id);

public class CreateLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/locations", async (CreateLocationRequest request, ISender sender) =>
        {
            var command = new CreateLocationCommand(
                request.TenantId, request.Name, request.Slug, request.Type,
                request.Address, request.Phone, request.Description, request.IsActive);

            var result = await sender.Send(command);
            var response = new CreateLocationResponse(result.Id);
            return Results.Created($"/locations/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithName("CreateLocation")
        .Produces<CreateLocationResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Location")
        .WithDescription("Create a new location for a tenant");
    }
}
