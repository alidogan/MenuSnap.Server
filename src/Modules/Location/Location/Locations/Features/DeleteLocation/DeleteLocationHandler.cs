using Location.Locations.Exceptions;

namespace Location.Locations.Features.DeleteLocation;

public record DeleteLocationCommand(Guid Id) : ICommand<DeleteLocationResult>;

public record DeleteLocationResult(bool IsSuccess);

internal class DeleteLocationHandler(LocationDbContext dbContext)
    : ICommandHandler<DeleteLocationCommand, DeleteLocationResult>
{
    public async Task<DeleteLocationResult> Handle(
        DeleteLocationCommand command, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == command.Id, cancellationToken);

        if (location is null)
            throw new LocationNotFoundException(command.Id);

        location.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteLocationResult(true);
    }
}
