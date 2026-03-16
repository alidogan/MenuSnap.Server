namespace Catalog.Contracts.Items.Dtos;

public record CatalogItemDto(
    Guid Id,
    Guid TenantId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    int? Calories,
    int? PrepTimeMinutes,
    bool IsAvailable,
    int DisplayOrder,
    IReadOnlyList<string> Allergens,
    IReadOnlyList<string> Badges);
