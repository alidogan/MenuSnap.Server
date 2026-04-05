namespace Tenant.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenants.Models.Tenant>
{
    public void Configure(EntityTypeBuilder<Tenants.Models.Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        builder.Property(t => t.LogoUrl).HasMaxLength(500);
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.DefaultLocale).HasMaxLength(10).HasDefaultValue("nl").IsRequired();
        builder.Property(t => t.SupportedLocales)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new(),
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.HasIndex(t => t.Slug).IsUnique();

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
