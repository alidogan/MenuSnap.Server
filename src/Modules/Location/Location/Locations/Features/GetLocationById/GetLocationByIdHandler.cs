using Location.Contracts.Locations.Features.GetLocationById;
using Location.Locations.Exceptions;
using Location.Locations.Features.CreateLocation;

namespace Location.Locations.Features.GetLocationById;

internal class GetLocationByIdHandler(LocationDbContext dbContext)
    : IQueryHandler<GetLocationByIdQuery, GetLocationByIdResult>
{
    public async Task<GetLocationByIdResult> Handle(
        GetLocationByIdQuery query, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .AsNoTracking()
            .SingleOrDefaultAsync(l => l.Id == query.Id, cancellationToken);

        if (location is null)
            throw new LocationNotFoundException(query.Id);

        return new GetLocationByIdResult(CreateLocationMapper.ToDto(location));
    }
}
