namespace Location.Data;

public class LocationDbContext(DbContextOptions<LocationDbContext> options) : DbContext(options)
{
    public DbSet<Locations.Models.Location> Locations => Set<Locations.Models.Location>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("location");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
