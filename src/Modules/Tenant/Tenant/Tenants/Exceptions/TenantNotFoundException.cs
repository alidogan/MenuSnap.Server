using Shared.Exceptions;

namespace Tenant.Tenants.Exceptions;

public class TenantNotFoundException : NotFoundException
{
    public TenantNotFoundException(Guid id)
        : base("Tenant", id)
    {
    }
}
