namespace ServiceUnit.Contracts.ServiceUnits.Dtos;

public record ServiceUnitDto(
    Guid Id,
    Guid LocationId,
    string Name,
    string Code,
    string Type,
    int? Capacity,
    string? GroupName,
    string? ExternalReference,
    int SortOrder,
    string Status,
    DateTime? LastUsedAt,
    bool IsActive,
    DateTime? CreatedAt,
    DateTime? LastModified);
