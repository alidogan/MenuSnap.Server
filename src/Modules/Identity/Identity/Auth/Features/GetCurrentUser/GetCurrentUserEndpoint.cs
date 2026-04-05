using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Auth.Features.GetCurrentUser;

public record CurrentUserResponse(
    Guid Id,
    string Email,
    string DisplayName,
    IList<string> Roles);

public class GetCurrentUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", async (
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager) =>
        {
            var userId = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                         ?? httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return Results.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            return Results.Ok(new CurrentUserResponse(
                user.Id, user.Email!, user.DisplayName, roles));
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .Produces<CurrentUserResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get Current User")
        .WithDescription("Returns the currently authenticated user's information");
    }
}
