using Tenant.Tenants.Exceptions;

namespace Tenant.Tenants.Features.UpdateTenant;

public record UpdateTenantCommand(Guid Id, string Name, string Slug, string? LogoUrl, bool IsActive)
    : ICommand<UpdateTenantResult>;

public record UpdateTenantResult(bool IsSuccess);

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required");
        RuleFor(x => x.Slug)
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .When(x => !string.IsNullOrEmpty(x.Slug))
            .WithMessage("Slug must be lowercase alphanumeric with hyphens");
    }
}

internal class UpdateTenantHandler(TenantDbContext dbContext)
    : ICommandHandler<UpdateTenantCommand, UpdateTenantResult>
{
    public async Task<UpdateTenantResult> Handle(
        UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await dbContext.Tenants
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (tenant is null)
            throw new TenantNotFoundException(command.Id);

        tenant.Update(command.Name, command.Slug, command.LogoUrl, command.IsActive);
        dbContext.Tenants.Update(tenant);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTenantResult(true);
    }
}
