namespace Catalog.Modifiers.Features.CreateModifierGroup;

public record CreateModifierGroupCommand(
    Guid TenantId,
    Guid ItemId,
    string Name,
    bool IsRequired,
    bool IsMultiSelect,
    int? MinSelections,
    int? MaxSelections,
    int DisplayOrder,
    bool IsActive = true,
    Dictionary<string, LocalizedContent>? Translations = null)
    : ICommand<CreateModifierGroupResult>;

public record CreateModifierGroupResult(Guid Id);

public class CreateModifierGroupCommandValidator : AbstractValidator<CreateModifierGroupCommand>
{
    public CreateModifierGroupCommandValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.MinSelections).GreaterThanOrEqualTo(1)
            .When(x => x.MinSelections.HasValue)
            .WithMessage("MinSelections must be at least 1");
        RuleFor(x => x).Must(x => !x.IsRequired || x.MinSelections.HasValue)
            .WithMessage("MinSelections must be specified when IsRequired is true");
        RuleFor(x => x).Must(x => !x.MaxSelections.HasValue || !x.MinSelections.HasValue || x.MaxSelections >= x.MinSelections)
            .WithMessage("MaxSelections cannot be less than MinSelections");
    }
}

internal class CreateModifierGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateModifierGroupCommand, CreateModifierGroupResult>
{
    public async Task<CreateModifierGroupResult> Handle(
        CreateModifierGroupCommand command, CancellationToken cancellationToken)
    {
        var group = Models.ItemModifierGroup.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            command.ItemId,
            command.Name,
            command.IsRequired,
            command.IsMultiSelect,
            command.MinSelections,
            command.MaxSelections,
            command.DisplayOrder,
            command.IsActive,
            command.Translations);

        dbContext.ItemModifierGroups.Add(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateModifierGroupResult(group.Id);
    }
}
