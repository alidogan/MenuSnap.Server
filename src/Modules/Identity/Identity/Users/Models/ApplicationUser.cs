using Microsoft.AspNetCore.Identity;

namespace Identity.Users.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = default!;
}
