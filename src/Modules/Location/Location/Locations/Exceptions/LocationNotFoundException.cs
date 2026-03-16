using Shared.Exceptions;

namespace Location.Locations.Exceptions;

public class LocationNotFoundException(Guid id)
    : NotFoundException($"Location with id '{id}' was not found.");
