using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Groups.Models.CatalogGroup> CatalogGroups => Set<Groups.Models.CatalogGroup>();
    public DbSet<Categories.Models.CatalogCategory> CatalogCategories => Set<Categories.Models.CatalogCategory>();
    public DbSet<Items.Models.CatalogItem> CatalogItems => Set<Items.Models.CatalogItem>();
    public DbSet<Modifiers.Models.ItemModifierGroup> ItemModifierGroups => Set<Modifiers.Models.ItemModifierGroup>();
    public DbSet<Modifiers.Models.ItemModifier> ItemModifiers => Set<Modifiers.Models.ItemModifier>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("catalog");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
