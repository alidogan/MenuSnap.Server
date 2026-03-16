using Shared.Exceptions;

namespace Catalog.Categories.Exceptions;

public class CatalogCategoryNotFoundException(Guid id)
    : NotFoundException($"CatalogCategory with id '{id}' was not found.");
