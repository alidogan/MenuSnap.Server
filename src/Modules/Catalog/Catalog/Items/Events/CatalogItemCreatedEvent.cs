namespace Catalog.Items.Events;

public record CatalogItemCreatedEvent(Items.Models.CatalogItem Item) : IDomainEvent;
