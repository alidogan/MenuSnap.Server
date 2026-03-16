namespace Location.Locations.Features.CreateLocation;

public record CreateLocationCommand(
    Guid TenantId,
    string Name,
    string Slug,
    string Type,
    string? Address,
    string? Phone,
    string? Description,
    bool IsActive = true)
    : ICommand<CreateLocationResult>;

public record CreateLocationResult(Guid Id);

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").When(x => !string.IsNullOrEmpty(x.Slug))
            .WithMessage("Slug must be lowercase alphanumeric with hyphens");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required")
            .Must(t => Enum.TryParse<Models.LocationType>(t, true, out _))
            .WithMessage("Type must be a valid LocationType");
    }
}

internal class CreateLocationHandler(LocationDbContext dbContext)
    : ICommandHandler<CreateLocationCommand, CreateLocationResult>
{
    public async Task<CreateLocationResult> Handle(
        CreateLocationCommand command, CancellationToken cancellationToken)
    {
        var location = CreateLocationMapper.ToEntity(command);
        dbContext.Locations.Add(location);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateLocationMapper.ToResult(location);
    }
}
