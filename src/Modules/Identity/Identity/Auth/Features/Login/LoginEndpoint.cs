namespace Identity.Auth.Features.Login;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string AccessToken, string RefreshToken);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request, ISender sender) =>
        {
            var result = await sender.Send(new LoginCommand(request.Email, request.Password));
            return Results.Ok(new LoginResponse(result.AccessToken, result.RefreshToken));
        })
        .AllowAnonymous()
        .WithName("Login")
        .Produces<LoginResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Login")
        .WithDescription("Authenticate with email and password, returns access and refresh tokens");
    }
}
