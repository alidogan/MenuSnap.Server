using ServiceUnit.ServiceUnits.Exceptions;

namespace ServiceUnit.ServiceUnits.Features.UpdateServiceUnit;

public record UpdateServiceUnitCommand(
    Guid Id,
    string Name,
    string Code,
    string Type,
    int? Capacity,
    string? GroupName,
    string? ExternalReference,
    int SortOrder,
    string Status,
    bool IsActive)
    : ICommand<UpdateServiceUnitResult>;

public record UpdateServiceUnitResult(bool IsSuccess);

public class UpdateServiceUnitCommandValidator : AbstractValidator<UpdateServiceUnitCommand>
{
    public UpdateServiceUnitCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").MaximumLength(50);
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required")
            .Must(t => Enum.TryParse<Models.ServiceUnitType>(t, true, out _))
            .WithMessage("Type must be a valid ServiceUnitType");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required")
            .Must(s => Enum.TryParse<Models.ServiceUnitStatus>(s, true, out _))
            .WithMessage("Status must be a valid ServiceUnitStatus");
        RuleFor(x => x.Capacity).GreaterThan(0).When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0");
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0).WithMessage("SortOrder must be >= 0");
    }
}

internal class UpdateServiceUnitHandler(ServiceUnitDbContext dbContext)
    : ICommandHandler<UpdateServiceUnitCommand, UpdateServiceUnitResult>
{
    public async Task<UpdateServiceUnitResult> Handle(
        UpdateServiceUnitCommand command, CancellationToken cancellationToken)
    {
        var serviceUnit = await dbContext.ServiceUnits
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);

        if (serviceUnit is null)
            throw new ServiceUnitNotFoundException(command.Id);

        serviceUnit.Update(
            command.Name,
            command.Code,
            Enum.Parse<Models.ServiceUnitType>(command.Type, ignoreCase: true),
            command.Capacity,
            command.GroupName,
            command.ExternalReference,
            command.SortOrder,
            Enum.Parse<Models.ServiceUnitStatus>(command.Status, ignoreCase: true),
            command.IsActive);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateServiceUnitResult(true);
    }
}
