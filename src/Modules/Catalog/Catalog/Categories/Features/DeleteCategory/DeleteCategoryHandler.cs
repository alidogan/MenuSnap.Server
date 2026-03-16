using Catalog.Categories.Exceptions;

namespace Catalog.Categories.Features.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand<DeleteCategoryResult>;
public record DeleteCategoryResult(bool IsSuccess);

internal class DeleteCategoryHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    public async Task<DeleteCategoryResult> Handle(
        DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await dbContext.CatalogCategories
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (category is null)
            throw new CatalogCategoryNotFoundException(command.Id);

        category.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteCategoryResult(true);
    }
}
