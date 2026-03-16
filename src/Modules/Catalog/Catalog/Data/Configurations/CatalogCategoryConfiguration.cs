using Catalog.Categories.Models;
using Catalog.Groups.Models;

namespace Catalog.Data.Configurations;

public class CatalogCategoryConfiguration : IEntityTypeConfiguration<CatalogCategory>
{
    public void Configure(EntityTypeBuilder<CatalogCategory> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500);

        builder.HasOne<CatalogGroup>()
            .WithMany()
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.GroupId);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
