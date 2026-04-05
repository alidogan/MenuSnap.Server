using Catalog.Modifiers.Models;
using Catalog.Shared;

namespace Catalog.UnitTests.Modifiers;

public class ItemModifierGroupTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsGroupWithCorrectProperties()
    {
        var itemId = Guid.CreateVersion7();

        var group = ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), itemId,
            "Extra Toppings", false, true, null, null, 1);

        group.ItemId.Should().Be(itemId);
        group.Name.Should().Be("Extra Toppings");
        group.IsRequired.Should().BeFalse();
        group.IsMultiSelect.Should().BeTrue();
        group.Translations.Should().BeEmpty();
    }

    [Fact]
    public void Create_Required_WithoutMinSelections_ThrowsArgumentException()
    {
        var act = () => ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Name", isRequired: true, isMultiSelect: false,
            minSelections: null, maxSelections: null, displayOrder: 1);

        act.Should().Throw<ArgumentException>().WithMessage("*MinSelections*");
    }

    [Fact]
    public void Create_WithMaxLessThanMin_ThrowsArgumentException()
    {
        var act = () => ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Name", isRequired: true, isMultiSelect: true,
            minSelections: 3, maxSelections: 2, displayOrder: 1);

        act.Should().Throw<ArgumentException>().WithMessage("*MaxSelections*");
    }

    [Fact]
    public void Create_WithEmptyItemId_ThrowsArgumentException()
    {
        var act = () => ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.Empty,
            "Name", false, false, null, null, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var group = ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Old", false, false, null, null, 1);

        group.Update("New", true, true, 1, 3, 2, false);

        group.Name.Should().Be("New");
        group.IsRequired.Should().BeTrue();
        group.MinSelections.Should().Be(1);
        group.MaxSelections.Should().Be(3);
        group.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Create_WithTranslations_StoresTranslationsCorrectly()
    {
        var translations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Extra Toppings", null),
            ["tr"] = new("Ekstra Malzemeler", null)
        };

        var group = ItemModifierGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Extra Toppings", false, true, null, null, 1,
            translations: translations);

        group.Translations.Should().HaveCount(2);
        group.Translations["en"].Name.Should().Be("Extra Toppings");
        group.Translations["tr"].Name.Should().Be("Ekstra Malzemeler");
    }
}
