using ServiceUnit.ServiceUnits.Exceptions;

namespace ServiceUnit.ServiceUnits.Features.DeleteServiceUnit;

public record DeleteServiceUnitCommand(Guid Id) : ICommand<DeleteServiceUnitResult>;

public record DeleteServiceUnitResult(bool IsSuccess);

internal class DeleteServiceUnitHandler(ServiceUnitDbContext dbContext)
    : ICommandHandler<DeleteServiceUnitCommand, DeleteServiceUnitResult>
{
    public async Task<DeleteServiceUnitResult> Handle(
        DeleteServiceUnitCommand command, CancellationToken cancellationToken)
    {
        var serviceUnit = await dbContext.ServiceUnits
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);

        if (serviceUnit is null)
            throw new ServiceUnitNotFoundException(command.Id);

        serviceUnit.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteServiceUnitResult(true);
    }
}
