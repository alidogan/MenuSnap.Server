using Catalog.Modifiers.Exceptions;

namespace Catalog.Modifiers.Features.DeleteModifierGroup;

public record DeleteModifierGroupCommand(Guid Id) : ICommand<DeleteModifierGroupResult>;
public record DeleteModifierGroupResult(bool IsSuccess);

internal class DeleteModifierGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteModifierGroupCommand, DeleteModifierGroupResult>
{
    public async Task<DeleteModifierGroupResult> Handle(
        DeleteModifierGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await dbContext.ItemModifierGroups
            .FirstOrDefaultAsync(g => g.Id == command.Id, cancellationToken);

        if (group is null)
            throw new ItemModifierGroupNotFoundException(command.Id);

        group.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteModifierGroupResult(true);
    }
}
