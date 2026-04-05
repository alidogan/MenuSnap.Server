using Catalog.Categories.Dtos;
using Catalog.Categories.Models;

namespace Catalog.Categories.Features.CreateCategory;

public static class CreateCategoryMapper
{
    public static CatalogCategory ToEntity(CreateCategoryCommand command) =>
        CatalogCategory.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            command.GroupId,
            command.Name,
            command.Description,
            command.DisplayOrder,
            command.IsActive,
            command.Translations);

    public static CreateCategoryResult ToResult(CatalogCategory category) => new(category.Id);

    public static CatalogCategoryDto ToDto(CatalogCategory category) =>
        new(category.Id, category.TenantId, category.GroupId, category.Name,
            category.Description, category.DisplayOrder, category.IsActive);
}
