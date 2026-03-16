namespace ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

public record CreateServiceUnitCommand(
    Guid LocationId,
    string Name,
    string Code,
    string Type,
    int? Capacity,
    string? GroupName,
    string? ExternalReference,
    int SortOrder,
    string? Status,
    bool IsActive = true)
    : ICommand<CreateServiceUnitResult>;

public record CreateServiceUnitResult(Guid Id);

public class CreateServiceUnitCommandValidator : AbstractValidator<CreateServiceUnitCommand>
{
    public CreateServiceUnitCommandValidator()
    {
        RuleFor(x => x.LocationId).NotEmpty().WithMessage("LocationId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(150);
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").MaximumLength(50);
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required")
            .Must(t => Enum.TryParse<Models.ServiceUnitType>(t, true, out _))
            .WithMessage("Type must be a valid ServiceUnitType");
        RuleFor(x => x.Status)
            .Must(s => s is null || Enum.TryParse<Models.ServiceUnitStatus>(s, true, out _))
            .WithMessage("Status must be a valid ServiceUnitStatus");
        RuleFor(x => x.Capacity).GreaterThan(0).When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0");
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0).WithMessage("SortOrder must be >= 0");
    }
}

internal class CreateServiceUnitHandler(ServiceUnitDbContext dbContext)
    : ICommandHandler<CreateServiceUnitCommand, CreateServiceUnitResult>
{
    public async Task<CreateServiceUnitResult> Handle(
        CreateServiceUnitCommand command, CancellationToken cancellationToken)
    {
        var serviceUnit = CreateServiceUnitMapper.ToEntity(command);
        dbContext.ServiceUnits.Add(serviceUnit);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateServiceUnitMapper.ToResult(serviceUnit);
    }
}
