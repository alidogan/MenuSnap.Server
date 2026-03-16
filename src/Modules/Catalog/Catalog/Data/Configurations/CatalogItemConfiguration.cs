using Catalog.Categories.Models;
using Catalog.Items.Models;

namespace Catalog.Data.Configurations;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).HasMaxLength(150).IsRequired();
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(i => i.ImageUrl).HasMaxLength(500);

        builder.Property(i => i.Allergens)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<Allergen>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<Allergen>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.Property(i => i.Badges)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.HasOne<CatalogCategory>()
            .WithMany()
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.CategoryId);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
