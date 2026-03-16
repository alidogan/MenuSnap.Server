using Catalog.Groups.Events;

namespace Catalog.Groups.Models;

public class CatalogGroup : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid LocationId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public CatalogGroupType Type { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    public static CatalogGroup Create(
        Guid id,
        Guid tenantId,
        Guid locationId,
        string name,
        string? description,
        CatalogGroupType type,
        int displayOrder,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));
        if (locationId == Guid.Empty) throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));

        var group = new CatalogGroup
        {
            Id = id,
            TenantId = tenantId,
            LocationId = locationId,
            Name = name,
            Description = description,
            Type = type,
            DisplayOrder = displayOrder,
            IsActive = isActive
        };

        group.AddDomainEvent(new CatalogGroupCreatedEvent(group));
        return group;
    }

    public void Update(
        string name,
        string? description,
        CatalogGroupType type,
        int displayOrder,
        bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        Type = type;
        DisplayOrder = displayOrder;
        IsActive = isActive;
    }
}
