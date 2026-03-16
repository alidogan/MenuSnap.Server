using Catalog.Items.Models;
using Catalog.Modifiers.Models;

namespace Catalog.Data.Configurations;

public class ItemModifierGroupConfiguration : IEntityTypeConfiguration<ItemModifierGroup>
{
    public void Configure(EntityTypeBuilder<ItemModifierGroup> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).HasMaxLength(150).IsRequired();

        builder.HasOne<CatalogItem>()
            .WithMany()
            .HasForeignKey(g => g.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.ItemId);

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}
