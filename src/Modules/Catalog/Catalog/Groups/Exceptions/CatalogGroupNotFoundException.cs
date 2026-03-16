using Shared.Exceptions;

namespace Catalog.Groups.Exceptions;

public class CatalogGroupNotFoundException(Guid id)
    : NotFoundException($"CatalogGroup with id '{id}' was not found.");
