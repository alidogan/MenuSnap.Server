namespace Tenant.Tenants.Features.CreateTenant;

public static class CreateTenantMapper
{
    public static Tenants.Models.Tenant ToEntity(CreateTenantCommand command) =>
        Tenants.Models.Tenant.Create(
            Guid.CreateVersion7(),
            command.Name,
            command.Slug,
            command.LogoUrl,
            command.IsActive);

    public static CreateTenantResult ToResult(Tenants.Models.Tenant tenant) =>
        new(tenant.Id);
}
