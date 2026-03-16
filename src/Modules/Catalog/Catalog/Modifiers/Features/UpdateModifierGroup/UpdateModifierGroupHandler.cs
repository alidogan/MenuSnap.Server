using Catalog.Modifiers.Exceptions;

namespace Catalog.Modifiers.Features.UpdateModifierGroup;

public record UpdateModifierGroupCommand(
    Guid Id, string Name,
    bool IsRequired, bool IsMultiSelect,
    int? MinSelections, int? MaxSelections,
    int DisplayOrder, bool IsActive)
    : ICommand<UpdateModifierGroupResult>;

public record UpdateModifierGroupResult(bool IsSuccess);

public class UpdateModifierGroupCommandValidator : AbstractValidator<UpdateModifierGroupCommand>
{
    public UpdateModifierGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x).Must(x => !x.IsRequired || x.MinSelections.HasValue)
            .WithMessage("MinSelections must be specified when IsRequired is true");
    }
}

internal class UpdateModifierGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateModifierGroupCommand, UpdateModifierGroupResult>
{
    public async Task<UpdateModifierGroupResult> Handle(
        UpdateModifierGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await dbContext.ItemModifierGroups
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (group is null)
            throw new ItemModifierGroupNotFoundException(command.Id);

        group.Update(command.Name, command.IsRequired, command.IsMultiSelect,
            command.MinSelections, command.MaxSelections, command.DisplayOrder, command.IsActive);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateModifierGroupResult(true);
    }
}
