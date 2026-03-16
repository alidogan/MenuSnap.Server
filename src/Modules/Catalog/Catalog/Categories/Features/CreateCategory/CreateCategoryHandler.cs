namespace Catalog.Categories.Features.CreateCategory;

public record CreateCategoryCommand(
    Guid TenantId,
    Guid GroupId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive = true)
    : ICommand<CreateCategoryResult>;

public record CreateCategoryResult(Guid Id);

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.GroupId).NotEmpty().WithMessage("GroupId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
    }
}

internal class CreateCategoryHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
{
    public async Task<CreateCategoryResult> Handle(
        CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = CreateCategoryMapper.ToEntity(command);
        dbContext.CatalogCategories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateCategoryMapper.ToResult(category);
    }
}
