namespace ServiceUnit.Data.Seed;

public class ServiceUnitDataSeeder(ServiceUnitDbContext dbContext) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
