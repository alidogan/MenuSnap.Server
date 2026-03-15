namespace Tenant.Tenants.Features.GetTenants;

public record GetTenantsQuery(PaginationRequest PaginationRequest)
    : IQuery<GetTenantsResult>;

public record GetTenantsResult(PaginatedResult<TenantDto> Tenants);

internal class GetTenantsHandler(TenantDbContext dbContext)
    : IQueryHandler<GetTenantsQuery, GetTenantsResult>
{
    public async Task<GetTenantsResult> Handle(
        GetTenantsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var total = await dbContext.Tenants.LongCountAsync(cancellationToken);

        var tenants = await dbContext.Tenants
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = tenants.Select(GetTenantsMapper.ToDto).ToList();

        return new GetTenantsResult(new PaginatedResult<TenantDto>(pageIndex, pageSize, total, dtos));
    }
}
