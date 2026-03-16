using Location.Locations.Events;

namespace Location.Locations.Models;

public class Location : Aggregate<Guid>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public LocationType Type { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; }

    public static Location Create(
        Guid id,
        Guid tenantId,
        string name,
        string slug,
        LocationType type,
        string? address,
        string? phone,
        string? description,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        var location = new Location
        {
            Id = id,
            TenantId = tenantId,
            Name = name,
            Slug = slug,
            Type = type,
            Address = address,
            Phone = phone,
            Description = description,
            IsActive = isActive
        };

        location.AddDomainEvent(new LocationCreatedEvent(location));

        return location;
    }

    public void Update(
        string name,
        string slug,
        LocationType type,
        string? address,
        string? phone,
        string? description,
        bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        Name = name;
        Slug = slug;
        Type = type;
        Address = address;
        Phone = phone;
        Description = description;
        IsActive = isActive;
    }

    public void SetLogoUrl(string logoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(logoUrl);
        LogoUrl = logoUrl;
    }
}
