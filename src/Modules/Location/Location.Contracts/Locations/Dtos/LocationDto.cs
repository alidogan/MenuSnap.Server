namespace Location.Contracts.Locations.Dtos;

public record LocationDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string Slug,
    string Type,
    string? Address,
    string? Phone,
    string? Description,
    string? LogoUrl,
    bool IsActive);
