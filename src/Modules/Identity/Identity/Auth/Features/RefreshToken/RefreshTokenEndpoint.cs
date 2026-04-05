namespace Identity.Auth.Features.RefreshToken;

public record RefreshTokenRequest(string RefreshToken);

public record RefreshTokenResponse(string AccessToken, string RefreshToken);

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (RefreshTokenRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RefreshTokenCommand(request.RefreshToken));
            return Results.Ok(new RefreshTokenResponse(result.AccessToken, result.RefreshToken));
        })
        .AllowAnonymous()
        .WithName("RefreshToken")
        .Produces<RefreshTokenResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Refresh Token")
        .WithDescription("Exchange a refresh token for a new access token and refresh token");
    }
}
