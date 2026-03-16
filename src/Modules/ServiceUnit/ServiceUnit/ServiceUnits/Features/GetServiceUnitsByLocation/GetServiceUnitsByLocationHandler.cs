using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

namespace ServiceUnit.ServiceUnits.Features.GetServiceUnitsByLocation;

public record GetServiceUnitsByLocationQuery(Guid LocationId) : IQuery<GetServiceUnitsByLocationResult>;

public record GetServiceUnitsByLocationResult(IReadOnlyList<ServiceUnitDto> ServiceUnits);

internal class GetServiceUnitsByLocationHandler(ServiceUnitDbContext dbContext)
    : IQueryHandler<GetServiceUnitsByLocationQuery, GetServiceUnitsByLocationResult>
{
    public async Task<GetServiceUnitsByLocationResult> Handle(
        GetServiceUnitsByLocationQuery query, CancellationToken cancellationToken)
    {
        var serviceUnits = await dbContext.ServiceUnits
            .AsNoTracking()
            .Where(s => s.LocationId == query.LocationId)
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return new GetServiceUnitsByLocationResult(
            serviceUnits.Select(CreateServiceUnitMapper.ToDto).ToList());
    }
}
