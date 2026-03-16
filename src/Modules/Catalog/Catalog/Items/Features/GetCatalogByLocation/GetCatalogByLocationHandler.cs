using Catalog.Modifiers.Dtos;

namespace Catalog.Items.Features.GetCatalogByLocation;

public record GetCatalogByLocationQuery(Guid LocationId) : IQuery<GetCatalogByLocationResult>;

public record GetCatalogByLocationResult(CatalogLocationView Catalog);

public record CatalogLocationView(
    Guid LocationId,
    IReadOnlyList<CatalogGroupView> Groups);

public record CatalogGroupView(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    int DisplayOrder,
    IReadOnlyList<CatalogCategoryView> Categories);

public record CatalogCategoryView(
    Guid Id,
    string Name,
    string? Description,
    int DisplayOrder,
    IReadOnlyList<CatalogItemView> Items);

public record CatalogItemView(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    int? Calories,
    int? PrepTimeMinutes,
    int DisplayOrder,
    IReadOnlyList<string> Allergens,
    IReadOnlyList<string> Badges,
    IReadOnlyList<ItemModifierGroupDto> ModifierGroups);

internal class GetCatalogByLocationHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetCatalogByLocationQuery, GetCatalogByLocationResult>
{
    public async Task<GetCatalogByLocationResult> Handle(
        GetCatalogByLocationQuery query, CancellationToken cancellationToken)
    {
        var groups = await dbContext.CatalogGroups
            .AsNoTracking()
            .Where(g => g.LocationId == query.LocationId && g.IsActive)
            .OrderBy(g => g.DisplayOrder)
            .ToListAsync(cancellationToken);

        var groupIds = groups.Select(g => g.Id).ToList();

        var categories = await dbContext.CatalogCategories
            .AsNoTracking()
            .Where(c => groupIds.Contains(c.GroupId) && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);

        var categoryIds = categories.Select(c => c.Id).ToList();

        var items = await dbContext.CatalogItems
            .AsNoTracking()
            .Where(i => categoryIds.Contains(i.CategoryId) && i.IsAvailable)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync(cancellationToken);

        var itemIds = items.Select(i => i.Id).ToList();

        var modifierGroups = await dbContext.ItemModifierGroups
            .AsNoTracking()
            .Where(mg => itemIds.Contains(mg.ItemId) && mg.IsActive)
            .OrderBy(mg => mg.DisplayOrder)
            .ToListAsync(cancellationToken);

        var modifierGroupIds = modifierGroups.Select(mg => mg.Id).ToList();

        var modifiers = await dbContext.ItemModifiers
            .AsNoTracking()
            .Where(m => modifierGroupIds.Contains(m.ModifierGroupId) && m.IsAvailable)
            .OrderBy(m => m.DisplayOrder)
            .ToListAsync(cancellationToken);

        var modifiersByGroup = modifiers
            .GroupBy(m => m.ModifierGroupId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<ItemModifierDto>)g.Select(m =>
                    new ItemModifierDto(m.Id, m.Name, m.PriceDelta, m.IsDefault, m.IsAvailable, m.DisplayOrder))
                    .ToList());

        var modifierGroupsByItem = modifierGroups
            .GroupBy(mg => mg.ItemId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<ItemModifierGroupDto>)g.Select(mg =>
                    new ItemModifierGroupDto(
                        mg.Id, mg.ItemId, mg.Name, mg.IsRequired, mg.IsMultiSelect,
                        mg.MinSelections, mg.MaxSelections, mg.DisplayOrder, mg.IsActive,
                        modifiersByGroup.GetValueOrDefault(mg.Id, [])))
                    .ToList());

        var itemsByCategory = items
            .GroupBy(i => i.CategoryId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<CatalogItemView>)g.Select(i =>
                    new CatalogItemView(
                        i.Id, i.Name, i.Description, i.Price, i.ImageUrl,
                        i.Calories, i.PrepTimeMinutes, i.DisplayOrder,
                        i.Allergens.Select(a => a.ToString()).ToList(),
                        i.Badges,
                        modifierGroupsByItem.GetValueOrDefault(i.Id, [])))
                    .ToList());

        var categoriesByGroup = categories
            .GroupBy(c => c.GroupId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<CatalogCategoryView>)g.Select(c =>
                    new CatalogCategoryView(
                        c.Id, c.Name, c.Description, c.DisplayOrder,
                        itemsByCategory.GetValueOrDefault(c.Id, [])))
                    .ToList());

        var groupViews = groups.Select(g =>
            new CatalogGroupView(
                g.Id, g.Name, g.Description, g.Type.ToString(), g.DisplayOrder,
                categoriesByGroup.GetValueOrDefault(g.Id, [])))
            .ToList();

        return new GetCatalogByLocationResult(
            new CatalogLocationView(query.LocationId, groupViews));
    }
}
