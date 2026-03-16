namespace Catalog.Items.Features.GetCatalogByLocation;

public class GetCatalogByLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/catalog/location/{locationId:guid}", async (Guid locationId, ISender sender) =>
        {
            var result = await sender.Send(new GetCatalogByLocationQuery(locationId));
            return Results.Ok(result.Catalog);
        })
        .AllowAnonymous()
        .WithName("GetCatalogByLocation")
        .Produces<CatalogLocationView>(StatusCodes.Status200OK)
        .WithSummary("Get full catalog for a location (public QR endpoint)");
    }
}
