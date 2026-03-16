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

        builder.HasIndex(t => t.Slug).IsUnique();

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
