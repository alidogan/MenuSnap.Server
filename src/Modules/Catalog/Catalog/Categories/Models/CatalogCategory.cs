using Catalog.Categories.Events;

namespace Catalog.Categories.Models;

public class CatalogCategory : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid GroupId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public Dictionary<string, LocalizedContent> Translations { get; private set; } = new();

    public static CatalogCategory Create(
        Guid id,
        Guid tenantId,
        Guid groupId,
        string name,
        string? description,
        int displayOrder,
        bool isActive = true,
        Dictionary<string, LocalizedContent>? translations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (groupId == Guid.Empty) throw new ArgumentException("GroupId cannot be empty.", nameof(groupId));

        var category = new CatalogCategory
        {
            Id = id,
            TenantId = tenantId,
            GroupId = groupId,
            Name = name,
            Description = description,
            DisplayOrder = displayOrder,
            IsActive = isActive,
            Translations = translations ?? new()
        };

        category.AddDomainEvent(new CatalogCategoryCreatedEvent(category));
        return category;
    }

    public void Update(string name, string? description, int displayOrder, bool isActive,
        Dictionary<string, LocalizedContent>? translations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        IsActive = isActive;
        Translations = translations ?? new();
    }
}
