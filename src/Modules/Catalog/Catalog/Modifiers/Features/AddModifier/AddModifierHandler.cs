using Catalog.Modifiers.Exceptions;

namespace Catalog.Modifiers.Features.AddModifier;

public record AddModifierCommand(
    Guid ModifierGroupId,
    string Name,
    decimal PriceDelta,
    bool IsDefault,
    bool IsAvailable,
    int DisplayOrder,
    Dictionary<string, LocalizedContent>? Translations = null)
    : ICommand<AddModifierResult>;

public record AddModifierResult(Guid Id);

public class AddModifierCommandValidator : AbstractValidator<AddModifierCommand>
{
    public AddModifierCommandValidator()
    {
        RuleFor(x => x.ModifierGroupId).NotEmpty().WithMessage("ModifierGroupId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
    }
}

internal class AddModifierHandler(CatalogDbContext dbContext)
    : ICommandHandler<AddModifierCommand, AddModifierResult>
{
    public async Task<AddModifierResult> Handle(
        AddModifierCommand command, CancellationToken cancellationToken)
    {
        var groupExists = await dbContext.ItemModifierGroups
            .AnyAsync(g => g.Id == command.ModifierGroupId, cancellationToken);

        if (!groupExists)
            throw new ItemModifierGroupNotFoundException(command.ModifierGroupId);

        var modifier = Models.ItemModifier.Create(
            Guid.CreateVersion7(),
            command.ModifierGroupId,
            command.Name,
            command.PriceDelta,
            command.IsDefault,
            command.IsAvailable,
            command.DisplayOrder,
            command.Translations);

        dbContext.ItemModifiers.Add(modifier);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new AddModifierResult(modifier.Id);
    }
}
