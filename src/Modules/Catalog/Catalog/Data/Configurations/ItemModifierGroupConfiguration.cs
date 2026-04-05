using Catalog.Items.Models;
using Catalog.Modifiers.Models;

namespace Catalog.Data.Configurations;

public class ItemModifierGroupConfiguration : IEntityTypeConfiguration<ItemModifierGroup>
{
    public void Configure(EntityTypeBuilder<ItemModifierGroup> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).HasMaxLength(150).IsRequired();

        builder.Property(g => g.Translations)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, LocalizedContent>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Dictionary<string, LocalizedContent>>(
                    (c1, c2) => System.Text.Json.JsonSerializer.Serialize(c1, (System.Text.Json.JsonSerializerOptions?)null) == System.Text.Json.JsonSerializer.Serialize(c2, (System.Text.Json.JsonSerializerOptions?)null),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    c => c.ToDictionary(k => k.Key, k => k.Value)));

        builder.HasOne<CatalogItem>()
            .WithMany()
            .HasForeignKey(g => g.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.ItemId);

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}
