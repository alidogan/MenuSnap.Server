namespace Catalog.Data.Seed;

public class CatalogDataSeeder(CatalogDbContext dbContext) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
