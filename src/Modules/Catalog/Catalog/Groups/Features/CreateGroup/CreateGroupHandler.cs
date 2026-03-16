using Catalog.Groups.Models;

namespace Catalog.Groups.Features.CreateGroup;

public record CreateGroupCommand(
    Guid TenantId,
    Guid LocationId,
    string Name,
    string? Description,
    string Type,
    int DisplayOrder,
    bool IsActive = true)
    : ICommand<CreateGroupResult>;

public record CreateGroupResult(Guid Id);

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
        RuleFor(x => x.LocationId).NotEmpty().WithMessage("LocationId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Type).NotEmpty()
            .Must(t => Enum.TryParse<CatalogGroupType>(t, true, out _))
            .WithMessage("Type must be a valid CatalogGroupType");
    }
}

internal class CreateGroupHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateGroupCommand, CreateGroupResult>
{
    public async Task<CreateGroupResult> Handle(
        CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var group = CreateGroupMapper.ToEntity(command);
        dbContext.CatalogGroups.Add(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateGroupMapper.ToResult(group);
    }
}
