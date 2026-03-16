using Location.Locations.Models;

namespace Location.Locations.Features.CreateLocation;

public static class CreateLocationMapper
{
    public static Locations.Models.Location ToEntity(CreateLocationCommand command) =>
        Locations.Models.Location.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            command.Name,
            command.Slug,
            Enum.Parse<LocationType>(command.Type, ignoreCase: true),
            command.Address,
            command.Phone,
            command.Description,
            command.IsActive);

    public static CreateLocationResult ToResult(Locations.Models.Location location) =>
        new(location.Id);

    public static LocationDto ToDto(Locations.Models.Location location) =>
        new(location.Id,
            location.TenantId,
            location.Name,
            location.Slug,
            location.Type.ToString(),
            location.Address,
            location.Phone,
            location.Description,
            location.LogoUrl,
            location.IsActive);
}
