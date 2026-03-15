namespace Tenant.Tenants.Features.GetTenants;

public static class GetTenantsMapper
{
    public static TenantDto ToDto(Tenants.Models.Tenant tenant) =>
        new(tenant.Id, tenant.Name, tenant.Slug, tenant.LogoUrl, tenant.IsActive);
}
