namespace Location.Data.Seed;

public class LocationDataSeeder(LocationDbContext dbContext) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
