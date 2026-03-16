namespace Catalog.Categories.Dtos;

public record CatalogCategoryDto(
    Guid Id,
    Guid TenantId,
    Guid GroupId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
