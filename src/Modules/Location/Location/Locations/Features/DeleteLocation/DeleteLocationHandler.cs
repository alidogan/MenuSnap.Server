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
            .FindAsync([command.Id], cancellationToken: cancellationToken);

        if (location is null)
            throw new LocationNotFoundException(command.Id);

        dbContext.Locations.Remove(location);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteLocationResult(true);
    }
}
