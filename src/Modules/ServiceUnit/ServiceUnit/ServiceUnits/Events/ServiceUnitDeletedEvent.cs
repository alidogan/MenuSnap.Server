namespace ServiceUnit.ServiceUnits.Events;

public record ServiceUnitDeletedEvent(Guid ServiceUnitId) : IDomainEvent;
