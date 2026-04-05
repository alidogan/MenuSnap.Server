namespace Identity.Contracts.TenantMembers.Dtos;

public record TenantMemberDto(
    Guid Id,
    Guid TenantId,
    Guid UserId,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive,
    DateTime? CreatedAt,
    DateTime? LastModified);
