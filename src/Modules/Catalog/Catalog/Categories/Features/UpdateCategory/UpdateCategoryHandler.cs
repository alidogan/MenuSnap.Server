using Catalog.Categories.Exceptions;

namespace Catalog.Categories.Features.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, string? Description, int DisplayOrder, bool IsActive)
    : ICommand<UpdateCategoryResult>;

public record UpdateCategoryResult(bool IsSuccess);

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}

internal class UpdateCategoryHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<UpdateCategoryResult> Handle(
        UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await dbContext.CatalogCategories
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (category is null)
            throw new CatalogCategoryNotFoundException(command.Id);

        category.Update(command.Name, command.Description, command.DisplayOrder, command.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateCategoryResult(true);
    }
}
