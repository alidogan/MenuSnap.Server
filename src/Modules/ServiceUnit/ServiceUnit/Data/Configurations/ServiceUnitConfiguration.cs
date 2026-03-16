using ServiceUnit.ServiceUnits.Models;

namespace ServiceUnit.Data.Configurations;

public class ServiceUnitConfiguration : IEntityTypeConfiguration<ServiceUnits.Models.ServiceUnit>
{
    public void Configure(EntityTypeBuilder<ServiceUnits.Models.ServiceUnit> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(150).IsRequired();
        builder.Property(s => s.Code).HasMaxLength(50).IsRequired();
        builder.Property(s => s.GroupName).HasMaxLength(100);
        builder.Property(s => s.ExternalReference).HasMaxLength(200);
        builder.Property(s => s.Type).HasConversion<string>().IsRequired();
        builder.Property(s => s.Status).HasConversion<string>().IsRequired();

        builder.HasIndex(s => s.LocationId);

        // Partial unique index: Code must be unique per Location among non-deleted records
        builder.HasIndex(s => new { s.LocationId, s.Code })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
