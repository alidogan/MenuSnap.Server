namespace ServiceUnit.Data;

public class ServiceUnitDbContext(DbContextOptions<ServiceUnitDbContext> options) : DbContext(options)
{
    public DbSet<ServiceUnits.Models.ServiceUnit> ServiceUnits => Set<ServiceUnits.Models.ServiceUnit>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("serviceunit");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
