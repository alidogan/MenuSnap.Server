using Catalog.Items.Models;

namespace Catalog.Items.Features.CreateItem;

public static class CreateItemMapper
{
    public static CatalogItem ToEntity(CreateItemCommand command)
    {
        var allergens = command.Allergens?
            .Select(a => Enum.Parse<Allergen>(a, ignoreCase: true))
            .ToList();

        return CatalogItem.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            command.CategoryId,
            command.Name,
            command.Description,
            command.Price,
            command.Calories,
            command.PrepTimeMinutes,
            command.IsAvailable,
            command.DisplayOrder,
            allergens,
            command.Badges);
    }

    public static CreateItemResult ToResult(CatalogItem item) => new(item.Id);

    public static CatalogItemDto ToDto(CatalogItem item) =>
        new(item.Id,
            item.TenantId,
            item.CategoryId,
            item.Name,
            item.Description,
            item.Price,
            item.ImageUrl,
            item.Calories,
            item.PrepTimeMinutes,
            item.IsAvailable,
            item.DisplayOrder,
            item.Allergens.Select(a => a.ToString()).ToList(),
            item.Badges);
}
