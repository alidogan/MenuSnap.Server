namespace Catalog.Items.Features.UploadItemPhoto;

public class UploadItemPhotoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/catalog/items/{id:guid}/photo", async (
                Guid id,
                IFormFile file,
                ISender sender,
                HttpContext httpContext) =>
            {
                var tenantIdClaim = httpContext.User.FindFirst("tenantId")?.Value
                    ?? httpContext.User.FindFirst("sub")?.Value
                    ?? Guid.Empty.ToString();

                Guid.TryParse(tenantIdClaim, out var tenantId);

                var result = await sender.Send(new UploadItemPhotoCommand(
                    id, tenantId,
                    file.OpenReadStream(),
                    file.ContentType,
                    file.FileName));

                return Results.Ok(new { result.ImageUrl });
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .WithName("UploadCatalogItemPhoto")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Upload Catalog Item Photo");
    }
}
