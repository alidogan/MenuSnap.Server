using Catalog.Modifiers.Models;

namespace Catalog.Data.Configurations;

public class ItemModifierConfiguration : IEntityTypeConfiguration<ItemModifier>
{
    public void Configure(EntityTypeBuilder<ItemModifier> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(150).IsRequired();
        builder.Property(m => m.PriceDelta).HasPrecision(18, 2).IsRequired();

        builder.HasOne<ItemModifierGroup>()
            .WithMany()
            .HasForeignKey(m => m.ModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.ModifierGroupId);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
