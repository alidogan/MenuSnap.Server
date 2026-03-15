namespace Tenant.Contracts.Tenants.Dtos;

public record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    string? LogoUrl,
    bool IsActive);
