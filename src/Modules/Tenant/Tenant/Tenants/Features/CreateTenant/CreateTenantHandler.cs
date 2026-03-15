namespace Tenant.Tenants.Features.CreateTenant;

public record CreateTenantCommand(string Name, string Slug, string? LogoUrl, bool IsActive)
    : ICommand<CreateTenantResult>;

public record CreateTenantResult(Guid Id);

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required");
        RuleFor(x => x.Slug)
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .When(x => !string.IsNullOrEmpty(x.Slug))
            .WithMessage("Slug must be lowercase alphanumeric with hyphens");
    }
}

internal class CreateTenantHandler(TenantDbContext dbContext)
    : ICommandHandler<CreateTenantCommand, CreateTenantResult>
{
    public async Task<CreateTenantResult> Handle(
        CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = CreateTenantMapper.ToEntity(command);
        dbContext.Tenants.Add(tenant);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateTenantMapper.ToResult(tenant);
    }
}
