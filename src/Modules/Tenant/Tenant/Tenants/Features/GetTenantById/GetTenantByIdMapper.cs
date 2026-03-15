namespace Tenant.Tenants.Features.GetTenantById;

public static class GetTenantByIdMapper
{
    public static TenantDto ToDto(Tenants.Models.Tenant tenant) =>
        new(tenant.Id, tenant.Name, tenant.Slug, tenant.LogoUrl, tenant.IsActive);
}
