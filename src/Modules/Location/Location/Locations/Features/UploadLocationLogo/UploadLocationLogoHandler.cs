using Location.Locations.Exceptions;
using Storage.Contracts;

namespace Location.Locations.Features.UploadLocationLogo;

public record UploadLocationLogoCommand(
    Guid LocationId,
    Guid TenantId,
    Stream FileStream,
    string ContentType,
    string FileName)
    : ICommand<UploadLocationLogoResult>;

public record UploadLocationLogoResult(string LogoUrl);

internal class UploadLocationLogoHandler(
    LocationDbContext dbContext,
    IStorageService storageService)
    : ICommandHandler<UploadLocationLogoCommand, UploadLocationLogoResult>
{
    public async Task<UploadLocationLogoResult> Handle(
        UploadLocationLogoCommand command, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .FindAsync([command.LocationId], cancellationToken: cancellationToken);

        if (location is null)
            throw new LocationNotFoundException(command.LocationId);

        var ext = Path.GetExtension(command.FileName);
        var key = $"logos/{command.LocationId}{ext}";

        var url = await storageService.UploadAsync(
            "location",
            command.TenantId.ToString(),
            key,
            command.FileStream,
            command.ContentType,
            cancellationToken);

        location.SetLogoUrl(url);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UploadLocationLogoResult(url);
    }
}
