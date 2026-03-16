using Catalog.Modifiers.Models;

namespace Catalog.UnitTests.Modifiers;

public class ItemModifierTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsModifierWithCorrectProperties()
    {
        var groupId = Guid.CreateVersion7();

        var modifier = ItemModifier.Create(
            Guid.CreateVersion7(), groupId,
            "Extra Chicken", 2.50m, false, true, 1);

        modifier.ModifierGroupId.Should().Be(groupId);
        modifier.Name.Should().Be("Extra Chicken");
        modifier.PriceDelta.Should().Be(2.50m);
        modifier.IsDefault.Should().BeFalse();
        modifier.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNegativePriceDelta_IsAllowed()
    {
        var modifier = ItemModifier.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Discount", -0.50m, false, true, 1);

        modifier.PriceDelta.Should().Be(-0.50m);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => ItemModifier.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "", 0m, false, true, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyModifierGroupId_ThrowsArgumentException()
    {
        var act = () => ItemModifier.Create(
            Guid.CreateVersion7(), Guid.Empty, "Name", 0m, false, true, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var modifier = ItemModifier.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Old", 1m, false, true, 1);

        modifier.Update("New", 3m, true, false, 2);

        modifier.Name.Should().Be("New");
        modifier.PriceDelta.Should().Be(3m);
        modifier.IsDefault.Should().BeTrue();
        modifier.IsAvailable.Should().BeFalse();
    }
}
