using Catalog.Modifiers.Exceptions;

namespace Catalog.Modifiers.Features.UpdateModifier;

public record UpdateModifierCommand(
    Guid Id, string Name, decimal PriceDelta,
    bool IsDefault, bool IsAvailable, int DisplayOrder,
    Dictionary<string, LocalizedContent>? Translations = null)
    : ICommand<UpdateModifierResult>;

public record UpdateModifierResult(bool IsSuccess);

public class UpdateModifierCommandValidator : AbstractValidator<UpdateModifierCommand>
{
    public UpdateModifierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}

internal class UpdateModifierHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateModifierCommand, UpdateModifierResult>
{
    public async Task<UpdateModifierResult> Handle(
        UpdateModifierCommand command, CancellationToken cancellationToken)
    {
        var modifier = await dbContext.ItemModifiers
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (modifier is null)
            throw new ItemModifierNotFoundException(command.Id);

        modifier.Update(command.Name, command.PriceDelta, command.IsDefault, command.IsAvailable, command.DisplayOrder,
            command.Translations);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateModifierResult(true);
    }
}
