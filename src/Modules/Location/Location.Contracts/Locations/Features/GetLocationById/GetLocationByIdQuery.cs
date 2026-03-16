using Location.Contracts.Locations.Dtos;
using Shared.Contracts.CQRS;

namespace Location.Contracts.Locations.Features.GetLocationById;

public record GetLocationByIdQuery(Guid Id) : IQuery<GetLocationByIdResult>;

public record GetLocationByIdResult(LocationDto Location);
