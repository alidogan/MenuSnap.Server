using ServiceUnit.ServiceUnits.Exceptions;
using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

namespace ServiceUnit.ServiceUnits.Features.GetServiceUnitById;

public record GetServiceUnitByIdQuery(Guid Id) : IQuery<GetServiceUnitByIdResult>;

public record GetServiceUnitByIdResult(ServiceUnitDto ServiceUnit);

internal class GetServiceUnitByIdHandler(ServiceUnitDbContext dbContext)
    : IQueryHandler<GetServiceUnitByIdQuery, GetServiceUnitByIdResult>
{
    public async Task<GetServiceUnitByIdResult> Handle(
        GetServiceUnitByIdQuery query, CancellationToken cancellationToken)
    {
        var serviceUnit = await dbContext.ServiceUnits
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.Id == query.Id, cancellationToken);

        if (serviceUnit is null)
            throw new ServiceUnitNotFoundException(query.Id);

        return new GetServiceUnitByIdResult(CreateServiceUnitMapper.ToDto(serviceUnit));
    }
}
