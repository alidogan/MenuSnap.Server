namespace Catalog.Categories.Events;

public record CatalogCategoryCreatedEvent(Categories.Models.CatalogCategory Category) : IDomainEvent;
