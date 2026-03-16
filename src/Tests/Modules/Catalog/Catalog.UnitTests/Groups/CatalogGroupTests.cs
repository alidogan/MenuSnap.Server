using Catalog.Groups.Models;

namespace Catalog.UnitTests.Groups;

public class CatalogGroupTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsGroupWithCorrectProperties()
    {
        var tenantId = Guid.CreateVersion7();
        var locationId = Guid.CreateVersion7();

        var group = CatalogGroup.Create(
            Guid.CreateVersion7(), tenantId, locationId,
            "Breakfast", "Morning food", CatalogGroupType.Food, 1);

        group.TenantId.Should().Be(tenantId);
        group.LocationId.Should().Be(locationId);
        group.Name.Should().Be("Breakfast");
        group.Description.Should().Be("Morning food");
        group.Type.Should().Be(CatalogGroupType.Food);
        group.DisplayOrder.Should().Be(1);
        group.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "", null, CatalogGroupType.Food, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyLocationId_ThrowsArgumentException()
    {
        var act = () => CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.Empty,
            "Name", null, CatalogGroupType.Food, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var group = CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Old", null, CatalogGroupType.Food, 1);

        group.Update("New", "Desc", CatalogGroupType.Beverage, 2, false);

        group.Name.Should().Be("New");
        group.Description.Should().Be("Desc");
        group.Type.Should().Be(CatalogGroupType.Beverage);
        group.DisplayOrder.Should().Be(2);
        group.IsActive.Should().BeFalse();
    }
}
