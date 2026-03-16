using Catalog.Items.Exceptions;

namespace Catalog.Items.Features.DeleteItem;

public record DeleteItemCommand(Guid Id) : ICommand<DeleteItemResult>;
public record DeleteItemResult(bool IsSuccess);

internal class DeleteItemHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteItemCommand, DeleteItemResult>
{
    public async Task<DeleteItemResult> Handle(
        DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var item = await dbContext.CatalogItems
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (item is null)
            throw new CatalogItemNotFoundException(command.Id);

        item.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteItemResult(true);
    }
}
