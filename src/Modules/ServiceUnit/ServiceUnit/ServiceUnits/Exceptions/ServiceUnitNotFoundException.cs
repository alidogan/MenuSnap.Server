using Shared.Exceptions;

namespace ServiceUnit.ServiceUnits.Exceptions;

public class ServiceUnitNotFoundException(Guid id)
    : NotFoundException($"ServiceUnit with id '{id}' was not found.");
