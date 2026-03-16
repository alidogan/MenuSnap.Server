using Catalog.Items.Exceptions;
using Storage.Contracts;

namespace Catalog.Items.Features.UploadItemPhoto;

public record UploadItemPhotoCommand(
    Guid ItemId,
    Guid TenantId,
    Stream FileStream,
    string ContentType,
    string FileName)
    : ICommand<UploadItemPhotoResult>;

public record UploadItemPhotoResult(string ImageUrl);

internal class UploadItemPhotoHandler(
    CatalogDbContext dbContext,
    IStorageService storageService)
    : ICommandHandler<UploadItemPhotoCommand, UploadItemPhotoResult>
{
    public async Task<UploadItemPhotoResult> Handle(
        UploadItemPhotoCommand command, CancellationToken cancellationToken)
    {
        var item = await dbContext.CatalogItems
            .FindAsync([command.ItemId], cancellationToken: cancellationToken);

        if (item is null)
            throw new CatalogItemNotFoundException(command.ItemId);

        var ext = Path.GetExtension(command.FileName);
        var key = $"items/{command.ItemId}{ext}";

        var url = await storageService.UploadAsync(
            "catalog",
            command.TenantId.ToString(),
            key,
            command.FileStream,
            command.ContentType,
            cancellationToken);

        item.SetImageUrl(url);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UploadItemPhotoResult(url);
    }
}
