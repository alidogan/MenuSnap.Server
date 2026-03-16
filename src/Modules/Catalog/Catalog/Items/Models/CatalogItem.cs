using Catalog.Items.Events;

namespace Catalog.Items.Models;

public class CatalogItem : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public int? Calories { get; private set; }
    public int? PrepTimeMinutes { get; private set; }
    public bool IsAvailable { get; private set; }
    public int DisplayOrder { get; private set; }
    public List<Allergen> Allergens { get; private set; } = [];
    public List<string> Badges { get; private set; } = [];

    public static CatalogItem Create(
        Guid id,
        Guid tenantId,
        Guid categoryId,
        string name,
        string? description,
        decimal price,
        int? calories,
        int? prepTimeMinutes,
        bool isAvailable,
        int displayOrder,
        List<Allergen>? allergens = null,
        List<string>? badges = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (categoryId == Guid.Empty) throw new ArgumentException("CategoryId cannot be empty.", nameof(categoryId));
        if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));

        var item = new CatalogItem
        {
            Id = id,
            TenantId = tenantId,
            CategoryId = categoryId,
            Name = name,
            Description = description,
            Price = price,
            Calories = calories,
            PrepTimeMinutes = prepTimeMinutes,
            IsAvailable = isAvailable,
            DisplayOrder = displayOrder,
            Allergens = allergens ?? [],
            Badges = badges ?? []
        };

        item.AddDomainEvent(new CatalogItemCreatedEvent(item));
        return item;
    }

    public void Update(
        string name,
        string? description,
        decimal price,
        int? calories,
        int? prepTimeMinutes,
        bool isAvailable,
        int displayOrder,
        List<Allergen>? allergens,
        List<string>? badges)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));

        Name = name;
        Description = description;
        Price = price;
        Calories = calories;
        PrepTimeMinutes = prepTimeMinutes;
        IsAvailable = isAvailable;
        DisplayOrder = displayOrder;
        Allergens = allergens ?? [];
        Badges = badges ?? [];
    }

    public void SetImageUrl(string imageUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl);
        ImageUrl = imageUrl;
    }
}
