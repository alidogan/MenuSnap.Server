namespace Shared.Messaging.Events;

public record IntegrationEvent
{
    public Guid EventId => Guid.CreateVersion7();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
}
