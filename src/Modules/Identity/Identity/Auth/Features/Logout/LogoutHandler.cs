using Identity.Auth.Services;

namespace Identity.Auth.Features.Logout;

public record LogoutCommand(string RefreshToken) : ICommand<LogoutResult>;

public record LogoutResult(bool Success);

internal class LogoutHandler(TokenService tokenService)
    : ICommandHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(
        LogoutCommand command, CancellationToken cancellationToken)
    {
        await tokenService.RevokeRefreshTokenAsync(command.RefreshToken, cancellationToken);
        return new LogoutResult(true);
    }
}
