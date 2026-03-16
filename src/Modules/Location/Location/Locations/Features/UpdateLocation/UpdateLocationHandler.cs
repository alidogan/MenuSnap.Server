using Location.Locations.Exceptions;

namespace Location.Locations.Features.UpdateLocation;

public record UpdateLocationCommand(
    Guid Id,
    string Name,
    string Slug,
    string Type,
    string? Address,
    string? Phone,
    string? Description,
    bool IsActive)
    : ICommand<UpdateLocationResult>;

public record UpdateLocationResult(bool IsSuccess);

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").When(x => !string.IsNullOrEmpty(x.Slug))
            .WithMessage("Slug must be lowercase alphanumeric with hyphens");
        RuleFor(x => x.Type).NotEmpty()
            .Must(t => Enum.TryParse<Models.LocationType>(t, true, out _))
            .WithMessage("Type must be a valid LocationType");
    }
}

internal class UpdateLocationHandler(LocationDbContext dbContext)
    : ICommandHandler<UpdateLocationCommand, UpdateLocationResult>
{
    public async Task<UpdateLocationResult> Handle(
        UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (location is null)
            throw new LocationNotFoundException(command.Id);

        location.Update(
            command.Name,
            command.Slug,
            Enum.Parse<Models.LocationType>(command.Type, ignoreCase: true),
            command.Address,
            command.Phone,
            command.Description,
            command.IsActive);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateLocationResult(true);
    }
}
