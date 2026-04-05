using Identity.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Data.Seed;

public class IdentityDataSeeder(UserManager<ApplicationUser> userManager) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (await userManager.FindByEmailAsync("admin@menusnap.nl") is not null)
            return;

        var admin = new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            UserName = "admin@menusnap.nl",
            Email = "admin@menusnap.nl",
            DisplayName = "Admin",
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(admin, "Admin123!");
    }
}
