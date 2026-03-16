using Catalog.Groups.Models;

namespace Catalog.Data.Configurations;

public class CatalogGroupConfiguration : IEntityTypeConfiguration<CatalogGroup>
{
    public void Configure(EntityTypeBuilder<CatalogGroup> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).HasMaxLength(150).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(500);
        builder.Property(g => g.Type).HasConversion<string>().IsRequired();

        builder.HasIndex(g => g.LocationId);
        builder.HasIndex(g => g.TenantId);
        builder.HasIndex(g => new { g.LocationId, g.DisplayOrder });
    }
}
