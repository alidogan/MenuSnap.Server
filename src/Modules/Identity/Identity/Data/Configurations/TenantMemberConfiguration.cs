using Identity.TenantMembers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Data.Configurations;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMember>
{
    public void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Email).HasMaxLength(256).IsRequired();
        builder.Property(m => m.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(m => m.Role).HasConversion<string>().IsRequired();

        builder.HasIndex(m => m.TenantId);
        builder.HasIndex(m => m.UserId);

        builder.HasIndex(m => new { m.TenantId, m.UserId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
