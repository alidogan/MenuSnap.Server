using ServiceUnit.ServiceUnits.Models;

namespace ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

public static class CreateServiceUnitMapper
{
    public static ServiceUnits.Models.ServiceUnit ToEntity(CreateServiceUnitCommand command) =>
        ServiceUnits.Models.ServiceUnit.Create(
            Guid.CreateVersion7(),
            command.LocationId,
            command.Name,
            command.Code,
            Enum.Parse<ServiceUnitType>(command.Type, ignoreCase: true),
            command.Capacity,
            command.GroupName,
            command.ExternalReference,
            command.SortOrder,
            command.Status is not null
                ? Enum.Parse<ServiceUnitStatus>(command.Status, ignoreCase: true)
                : ServiceUnitStatus.Available,
            command.IsActive);

    public static CreateServiceUnitResult ToResult(ServiceUnits.Models.ServiceUnit serviceUnit) =>
        new(serviceUnit.Id);

    public static ServiceUnitDto ToDto(ServiceUnits.Models.ServiceUnit serviceUnit) =>
        new(serviceUnit.Id,
            serviceUnit.LocationId,
            serviceUnit.Name,
            serviceUnit.Code,
            serviceUnit.Type.ToString(),
            serviceUnit.Capacity,
            serviceUnit.GroupName,
            serviceUnit.ExternalReference,
            serviceUnit.SortOrder,
            serviceUnit.Status.ToString(),
            serviceUnit.LastUsedAt,
            serviceUnit.IsActive,
            serviceUnit.CreatedAt,
            serviceUnit.LastModified);
}
