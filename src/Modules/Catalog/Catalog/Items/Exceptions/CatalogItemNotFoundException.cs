using Shared.Exceptions;

namespace Catalog.Items.Exceptions;

public class CatalogItemNotFoundException(Guid id)
    : NotFoundException($"CatalogItem with id '{id}' was not found.");
