using Catalog.Modifiers.Exceptions;

namespace Catalog.Modifiers.Features.DeleteModifier;

public record DeleteModifierCommand(Guid Id) : ICommand<DeleteModifierResult>;
public record DeleteModifierResult(bool IsSuccess);

internal class DeleteModifierHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteModifierCommand, DeleteModifierResult>
{
    public async Task<DeleteModifierResult> Handle(
        DeleteModifierCommand command, CancellationToken cancellationToken)
    {
        var modifier = await dbContext.ItemModifiers
            .FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (modifier is null)
            throw new ItemModifierNotFoundException(command.Id);

        modifier.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteModifierResult(true);
    }
}
