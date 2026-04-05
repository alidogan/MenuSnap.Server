using Identity.Auth.Services;
using Identity.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Auth.Features.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResult>;

public record RefreshTokenResult(string AccessToken, string RefreshToken);

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

internal class RefreshTokenHandler(
    UserManager<ApplicationUser> userManager,
    TokenService tokenService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(
        RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var existing = await tokenService.ValidateRefreshTokenAsync(command.RefreshToken, cancellationToken);
        if (existing is null)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        var user = await userManager.FindByIdAsync(existing.UserId.ToString());
        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        await tokenService.RevokeRefreshTokenAsync(command.RefreshToken, cancellationToken);

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new RefreshTokenResult(accessToken, newRefreshToken.Token);
    }
}
