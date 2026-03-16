namespace Location.Locations.Events;

public record LocationCreatedEvent(Location.Locations.Models.Location Location) : IDomainEvent;
