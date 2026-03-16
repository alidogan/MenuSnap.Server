namespace Catalog.Groups.Dtos;

public record CatalogGroupDto(
    Guid Id,
    Guid TenantId,
    Guid LocationId,
    string Name,
    string? Description,
    string Type,
    int DisplayOrder,
    bool IsActive);
