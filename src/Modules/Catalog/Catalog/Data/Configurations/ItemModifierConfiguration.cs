using Catalog.Modifiers.Models;

namespace Catalog.Data.Configurations;

public class ItemModifierConfiguration : IEntityTypeConfiguration<ItemModifier>
{
    public void Configure(EntityTypeBuilder<ItemModifier> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(150).IsRequired();
        builder.Property(m => m.PriceDelta).HasPrecision(18, 2).IsRequired();

        builder.Property(m => m.Translations)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, LocalizedContent>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Dictionary<string, LocalizedContent>>(
                    (c1, c2) => System.Text.Json.JsonSerializer.Serialize(c1, (System.Text.Json.JsonSerializerOptions?)null) == System.Text.Json.JsonSerializer.Serialize(c2, (System.Text.Json.JsonSerializerOptions?)null),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    c => c.ToDictionary(k => k.Key, k => k.Value)));

        builder.HasOne<ItemModifierGroup>()
            .WithMany()
            .HasForeignKey(m => m.ModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.ModifierGroupId);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
