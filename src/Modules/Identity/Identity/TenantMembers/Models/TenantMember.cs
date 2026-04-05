namespace Identity.TenantMembers.Models;

public class TenantMember : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public TenantRole Role { get; private set; }
    public bool IsActive { get; private set; }

    public static TenantMember Create(
        Guid id,
        Guid tenantId,
        Guid userId,
        string email,
        string displayName,
        TenantRole role,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        return new TenantMember
        {
            Id = id,
            TenantId = tenantId,
            UserId = userId,
            Email = email,
            DisplayName = displayName,
            Role = role,
            IsActive = isActive
        };
    }

    public void Update(string displayName, TenantRole role, bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        DisplayName = displayName;
        Role = role;
        IsActive = isActive;
    }

    public override void Delete()
    {
        base.Delete();
    }
}
