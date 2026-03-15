namespace Tenant.Tenants.Events;

public record TenantCreatedEvent(Tenants.Models.Tenant Tenant) : IDomainEvent;
