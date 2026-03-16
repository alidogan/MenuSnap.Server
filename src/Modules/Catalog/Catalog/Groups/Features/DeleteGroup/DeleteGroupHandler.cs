using Catalog.Groups.Exceptions;

namespace Catalog.Groups.Features.DeleteGroup;

public record DeleteGroupCommand(Guid Id) : ICommand<DeleteGroupResult>;
public record DeleteGroupResult(bool IsSuccess);

internal class DeleteGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteGroupCommand, DeleteGroupResult>
{
    public async Task<DeleteGroupResult> Handle(
        DeleteGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await dbContext.CatalogGroups
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (group is null)
            throw new CatalogGroupNotFoundException(command.Id);

        dbContext.CatalogGroups.Remove(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteGroupResult(true);
    }
}
