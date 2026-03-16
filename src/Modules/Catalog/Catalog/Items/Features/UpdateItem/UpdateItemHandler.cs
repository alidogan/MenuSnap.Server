using Catalog.Items.Exceptions;
using Catalog.Items.Models;

namespace Catalog.Items.Features.UpdateItem;

public record UpdateItemCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int? Calories,
    int? PrepTimeMinutes,
    bool IsAvailable,
    int DisplayOrder,
    List<string>? Allergens,
    List<string>? Badges)
    : ICommand<UpdateItemResult>;

public record UpdateItemResult(bool IsSuccess);

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");
    }
}

internal class UpdateItemHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateItemCommand, UpdateItemResult>
{
    public async Task<UpdateItemResult> Handle(
        UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await dbContext.CatalogItems
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (item is null)
            throw new CatalogItemNotFoundException(command.Id);

        var allergens = command.Allergens?
            .Select(a => Enum.Parse<Allergen>(a, ignoreCase: true))
            .ToList();

        item.Update(
            command.Name, command.Description, command.Price, command.Calories,
            command.PrepTimeMinutes, command.IsAvailable, command.DisplayOrder,
            allergens, command.Badges);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateItemResult(true);
    }
}
