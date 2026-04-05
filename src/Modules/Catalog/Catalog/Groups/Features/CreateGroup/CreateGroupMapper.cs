using Catalog.Groups.Dtos;
using Catalog.Groups.Models;

namespace Catalog.Groups.Features.CreateGroup;

public static class CreateGroupMapper
{
    public static CatalogGroup ToEntity(CreateGroupCommand command) =>
        CatalogGroup.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            command.LocationId,
            command.Name,
            command.Description,
            Enum.Parse<CatalogGroupType>(command.Type, ignoreCase: true),
            command.DisplayOrder,
            command.IsActive,
            command.Translations);

    public static CreateGroupResult ToResult(CatalogGroup group) => new(group.Id);

    public static CatalogGroupDto ToDto(CatalogGroup group) =>
        new(group.Id, group.TenantId, group.LocationId, group.Name,
            group.Description, group.Type.ToString(), group.DisplayOrder, group.IsActive);
}
