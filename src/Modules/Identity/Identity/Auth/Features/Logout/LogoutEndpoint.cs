namespace Identity.Auth.Features.Logout;

public record LogoutRequest(string RefreshToken);

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (LogoutRequest request, ISender sender) =>
        {
            await sender.Send(new LogoutCommand(request.RefreshToken));
            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("Logout")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Logout")
        .WithDescription("Revoke a refresh token");
    }
}
