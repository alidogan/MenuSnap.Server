using Catalog.Shared;

namespace Catalog.Modifiers.Dtos;

public record ItemModifierGroupDto(
    Guid Id,
    Guid ItemId,
    string Name,
    bool IsRequired,
    bool IsMultiSelect,
    int? MinSelections,
    int? MaxSelections,
    int DisplayOrder,
    bool IsActive,
    Dictionary<string, LocalizedContent> Translations,
    IReadOnlyList<ItemModifierDto> Modifiers);

public record ItemModifierDto(
    Guid Id,
    string Name,
    decimal PriceDelta,
    bool IsDefault,
    bool IsAvailable,
    int DisplayOrder,
    Dictionary<string, LocalizedContent> Translations);
