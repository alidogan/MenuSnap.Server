namespace ServiceUnit.ServiceUnits.Events;

public record ServiceUnitCreatedEvent(ServiceUnits.Models.ServiceUnit ServiceUnit) : IDomainEvent;
