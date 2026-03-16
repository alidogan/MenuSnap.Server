using Location.Locations.Features.CreateLocation;

namespace Location.Locations.Features.GetLocationsByTenant;

public record GetLocationsByTenantQuery(Guid TenantId) : IQuery<GetLocationsByTenantResult>;

public record GetLocationsByTenantResult(IReadOnlyList<LocationDto> Locations);

public record GetLocationsByTenantResponse(IReadOnlyList<LocationDto> Locations);

internal class GetLocationsByTenantHandler(LocationDbContext dbContext)
    : IQueryHandler<GetLocationsByTenantQuery, GetLocationsByTenantResult>
{
    public async Task<GetLocationsByTenantResult> Handle(
        GetLocationsByTenantQuery query, CancellationToken cancellationToken)
    {
        var locations = await dbContext.Locations
            .AsNoTracking()
            .Where(l => l.TenantId == query.TenantId)
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return new GetLocationsByTenantResult(
            locations.Select(CreateLocationMapper.ToDto).ToList());
    }
}
