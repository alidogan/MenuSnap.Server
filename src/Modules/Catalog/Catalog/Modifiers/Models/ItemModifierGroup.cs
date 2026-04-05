namespace Catalog.Modifiers.Models;

public class ItemModifierGroup : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid ItemId { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsRequired { get; private set; }
    public bool IsMultiSelect { get; private set; }
    public int? MinSelections { get; private set; }
    public int? MaxSelections { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public Dictionary<string, LocalizedContent> Translations { get; private set; } = new();

    public static ItemModifierGroup Create(
        Guid id,
        Guid tenantId,
        Guid itemId,
        string name,
        bool isRequired,
        bool isMultiSelect,
        int? minSelections,
        int? maxSelections,
        int displayOrder,
        bool isActive = true,
        Dictionary<string, LocalizedContent>? translations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (itemId == Guid.Empty) throw new ArgumentException("ItemId cannot be empty.", nameof(itemId));

        if (isRequired && minSelections is null)
            throw new ArgumentException("MinSelections must be specified when IsRequired is true.", nameof(minSelections));

        if (minSelections.HasValue && minSelections.Value < 1)
            throw new ArgumentException("MinSelections must be at least 1.", nameof(minSelections));

        if (maxSelections.HasValue && minSelections.HasValue && maxSelections.Value < minSelections.Value)
            throw new ArgumentException("MaxSelections cannot be less than MinSelections.", nameof(maxSelections));

        return new ItemModifierGroup
        {
            Id = id,
            TenantId = tenantId,
            ItemId = itemId,
            Name = name,
            IsRequired = isRequired,
            IsMultiSelect = isMultiSelect,
            MinSelections = minSelections,
            MaxSelections = maxSelections,
            DisplayOrder = displayOrder,
            IsActive = isActive,
            Translations = translations ?? new()
        };
    }

    public void Update(
        string name,
        bool isRequired,
        bool isMultiSelect,
        int? minSelections,
        int? maxSelections,
        int displayOrder,
        bool isActive,
        Dictionary<string, LocalizedContent>? translations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (isRequired && minSelections is null)
            throw new ArgumentException("MinSelections must be specified when IsRequired is true.", nameof(minSelections));

        if (maxSelections.HasValue && minSelections.HasValue && maxSelections.Value < minSelections.Value)
            throw new ArgumentException("MaxSelections cannot be less than MinSelections.", nameof(maxSelections));

        Name = name;
        IsRequired = isRequired;
        IsMultiSelect = isMultiSelect;
        MinSelections = minSelections;
        MaxSelections = maxSelections;
        DisplayOrder = displayOrder;
        IsActive = isActive;
        Translations = translations ?? new();
    }
}
