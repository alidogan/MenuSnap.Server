namespace Location.Locations.Features.UploadLocationLogo;

public class UploadLocationLogoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/locations/{id:guid}/logo", async (
                Guid id,
                IFormFile file,
                ISender sender,
                HttpContext httpContext) =>
            {
                var tenantIdClaim = httpContext.User.FindFirst("tenantId")?.Value
                    ?? httpContext.User.FindFirst("sub")?.Value
                    ?? Guid.Empty.ToString();

                Guid.TryParse(tenantIdClaim, out var tenantId);

                var result = await sender.Send(new UploadLocationLogoCommand(
                    id,
                    tenantId,
                    file.OpenReadStream(),
                    file.ContentType,
                    file.FileName));

                return Results.Ok(new { result.LogoUrl });
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .WithName("UploadLocationLogo")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Upload Location Logo");
    }
}
