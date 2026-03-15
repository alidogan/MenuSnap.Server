using Tenant.Contracts.Tenants.Features.GetTenantById;
using Tenant.Tenants.Exceptions;

namespace Tenant.Tenants.Features.GetTenantById;

internal class GetTenantByIdHandler(TenantDbContext dbContext)
    : IQueryHandler<GetTenantByIdQuery, GetTenantByIdResult>
{
    public async Task<GetTenantByIdResult> Handle(
        GetTenantByIdQuery query, CancellationToken cancellationToken)
    {
        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == query.Id, cancellationToken);

        if (tenant is null)
            throw new TenantNotFoundException(query.Id);

        return new GetTenantByIdResult(GetTenantByIdMapper.ToDto(tenant));
    }
}
