namespace Tenant.Data;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
    public DbSet<Tenants.Models.Tenant> Tenants => Set<Tenants.Models.Tenant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("tenant");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
