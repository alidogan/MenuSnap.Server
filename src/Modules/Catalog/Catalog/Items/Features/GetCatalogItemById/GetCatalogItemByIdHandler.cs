using Catalog.Contracts.Items.Features.GetCatalogItemById;
using Catalog.Items.Exceptions;
using Catalog.Items.Features.CreateItem;

namespace Catalog.Items.Features.GetCatalogItemById;

internal class GetCatalogItemByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetCatalogItemByIdQuery, GetCatalogItemByIdResult>
{
    public async Task<GetCatalogItemByIdResult> Handle(
        GetCatalogItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await dbContext.CatalogItems
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

        if (item is null)
            throw new CatalogItemNotFoundException(query.Id);

        return new GetCatalogItemByIdResult(CreateItemMapper.ToDto(item));
    }
}
