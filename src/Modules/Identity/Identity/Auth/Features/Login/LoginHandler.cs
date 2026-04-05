using Identity.Auth.Services;
using Identity.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Auth.Features.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;

public record LoginResult(string AccessToken, string RefreshToken);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

internal class LoginHandler(
    UserManager<ApplicationUser> userManager,
    TokenService tokenService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(
        LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, command.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

        return new LoginResult(accessToken, refreshToken.Token);
    }
}
