using MediatR;

namespace Shared.DDD;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.CreateVersion7();
    DateTime OccurredOn => DateTime.UtcNow;
    string EventType => GetType().AssemblyQualifiedName!;
}
