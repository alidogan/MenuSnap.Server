using Shared.Exceptions;

namespace Catalog.Modifiers.Exceptions;

public class ItemModifierGroupNotFoundException(Guid id)
    : NotFoundException($"ItemModifierGroup with id '{id}' was not found.");

public class ItemModifierNotFoundException(Guid id)
    : NotFoundException($"ItemModifier with id '{id}' was not found.");
