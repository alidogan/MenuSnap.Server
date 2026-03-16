using Catalog.Contracts.Items.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Contracts.Items.Features.GetCatalogItemById;

public record GetCatalogItemByIdQuery(Guid Id) : IQuery<GetCatalogItemByIdResult>;

public record GetCatalogItemByIdResult(CatalogItemDto Item);
