using Catalog.Groups.Exceptions;
using Catalog.Groups.Models;

namespace Catalog.Groups.Features.UpdateGroup;

public record UpdateGroupCommand(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    int DisplayOrder,
    bool IsActive,
    Dictionary<string, LocalizedContent>? Translations = null)
    : ICommand<UpdateGroupResult>;

public record UpdateGroupResult(bool IsSuccess);

public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Type).NotEmpty()
            .Must(t => Enum.TryParse<CatalogGroupType>(t, true, out _))
            .WithMessage("Type must be a valid CatalogGroupType");
    }
}

internal class UpdateGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateGroupCommand, UpdateGroupResult>
{
    public async Task<UpdateGroupResult> Handle(
        UpdateGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await dbContext.CatalogGroups
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (group is null)
            throw new CatalogGroupNotFoundException(command.Id);

        group.Update(
            command.Name,
            command.Description,
            Enum.Parse<CatalogGroupType>(command.Type, ignoreCase: true),
            command.DisplayOrder,
            command.IsActive,
            command.Translations);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateGroupResult(true);
    }
}
