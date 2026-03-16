using ServiceUnit.ServiceUnits.Events;

namespace ServiceUnit.ServiceUnits.Models;

public class ServiceUnit : Aggregate<Guid>
{
    public Guid LocationId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public ServiceUnitType Type { get; private set; }
    public int? Capacity { get; private set; }
    public string? GroupName { get; private set; }
    public string? ExternalReference { get; private set; }
    public int SortOrder { get; private set; }
    public ServiceUnitStatus Status { get; private set; }
    public DateTime? LastUsedAt { get; private set; }
    public bool IsActive { get; private set; }

    public static ServiceUnit Create(
        Guid id,
        Guid locationId,
        string name,
        string code,
        ServiceUnitType type,
        int? capacity,
        string? groupName,
        string? externalReference,
        int sortOrder,
        ServiceUnitStatus status = ServiceUnitStatus.Available,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        if (locationId == Guid.Empty) throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));

        var serviceUnit = new ServiceUnit
        {
            Id = id,
            LocationId = locationId,
            Name = name,
            Code = code,
            Type = type,
            Capacity = capacity,
            GroupName = groupName,
            ExternalReference = externalReference,
            SortOrder = sortOrder,
            Status = status,
            IsActive = isActive
        };

        serviceUnit.AddDomainEvent(new ServiceUnitCreatedEvent(serviceUnit));

        return serviceUnit;
    }

    public void Update(
        string name,
        string code,
        ServiceUnitType type,
        int? capacity,
        string? groupName,
        string? externalReference,
        int sortOrder,
        ServiceUnitStatus status,
        bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        Name = name;
        Code = code;
        Type = type;
        Capacity = capacity;
        GroupName = groupName;
        ExternalReference = externalReference;
        SortOrder = sortOrder;
        Status = status;
        IsActive = isActive;
    }

    public void MarkUsed()
    {
        LastUsedAt = DateTime.UtcNow;
    }

    public override void Delete()
    {
        base.Delete();
        AddDomainEvent(new ServiceUnitDeletedEvent(Id));
    }
}
