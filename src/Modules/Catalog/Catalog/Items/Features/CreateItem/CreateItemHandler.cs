using Catalog.Items.Models;

namespace Catalog.Items.Features.CreateItem;

public record CreateItemCommand(
    Guid TenantId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int? Calories,
    int? PrepTimeMinutes,
    bool IsAvailable,
    int DisplayOrder,
    List<string>? Allergens,
    List<string>? Badges,
    Dictionary<string, LocalizedContent>? Translations = null)
    : ICommand<CreateItemResult>;

public record CreateItemResult(Guid Id);

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");
    }
}

internal class CreateItemHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateItemCommand, CreateItemResult>
{
    public async Task<CreateItemResult> Handle(
        CreateItemCommand command, CancellationToken cancellationToken)
    {
        var item = CreateItemMapper.ToEntity(command);
        dbContext.CatalogItems.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateItemMapper.ToResult(item);
    }
}
