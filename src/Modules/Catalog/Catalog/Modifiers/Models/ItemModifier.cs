namespace Catalog.Modifiers.Models;

public class ItemModifier : Aggregate<Guid>
{
    public Guid ModifierGroupId { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal PriceDelta { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsAvailable { get; private set; }
    public int DisplayOrder { get; private set; }

    public static ItemModifier Create(
        Guid id,
        Guid modifierGroupId,
        string name,
        decimal priceDelta,
        bool isDefault,
        bool isAvailable,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (modifierGroupId == Guid.Empty)
            throw new ArgumentException("ModifierGroupId cannot be empty.", nameof(modifierGroupId));

        return new ItemModifier
        {
            Id = id,
            ModifierGroupId = modifierGroupId,
            Name = name,
            PriceDelta = priceDelta,
            IsDefault = isDefault,
            IsAvailable = isAvailable,
            DisplayOrder = displayOrder
        };
    }

    public void Update(string name, decimal priceDelta, bool isDefault, bool isAvailable, int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        PriceDelta = priceDelta;
        IsDefault = isDefault;
        IsAvailable = isAvailable;
        DisplayOrder = displayOrder;
    }
}
