using Location.Locations.Models;

namespace Location.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Locations.Models.Location>
{
    public void Configure(EntityTypeBuilder<Locations.Models.Location> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Name).HasMaxLength(150).IsRequired();
        builder.Property(l => l.Slug).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Address).HasMaxLength(300);
        builder.Property(l => l.Phone).HasMaxLength(50);
        builder.Property(l => l.Description).HasMaxLength(1000);
        builder.Property(l => l.LogoUrl).HasMaxLength(500);
        builder.Property(l => l.Type).HasConversion<string>().IsRequired();

        builder.HasIndex(l => l.TenantId);
        builder.HasIndex(l => l.Slug).IsUnique();

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}
