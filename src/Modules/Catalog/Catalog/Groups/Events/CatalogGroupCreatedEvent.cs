namespace Catalog.Groups.Events;

public record CatalogGroupCreatedEvent(Groups.Models.CatalogGroup Group) : IDomainEvent;
