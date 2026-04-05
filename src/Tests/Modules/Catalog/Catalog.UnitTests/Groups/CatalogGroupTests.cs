using Catalog.Groups.Models;
using Catalog.Shared;

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
        group.Translations.Should().BeEmpty();
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

    [Fact]
    public void Create_WithTranslations_StoresTranslationsCorrectly()
    {
        var translations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Breakfast", "Morning meals"),
            ["tr"] = new("Kahvaltı", "Sabah yemekleri")
        };

        var group = CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Ontbijt", "Ochtend maaltijden", CatalogGroupType.Food, 1,
            translations: translations);

        group.Translations.Should().HaveCount(2);
        group.Translations["en"].Name.Should().Be("Breakfast");
        group.Translations["tr"].Description.Should().Be("Sabah yemekleri");
    }

    [Fact]
    public void Update_WithTranslations_ReplacesTranslations()
    {
        var group = CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Old", null, CatalogGroupType.Food, 1,
            translations: new Dictionary<string, LocalizedContent>
            {
                ["en"] = new("Old English", null)
            });

        var newTranslations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Drinks", "All beverages"),
            ["tr"] = new("İçecekler", "Tüm içecekler")
        };

        group.Update("Dranken", "Alle dranken", CatalogGroupType.Beverage, 2, true, newTranslations);

        group.Translations.Should().HaveCount(2);
        group.Translations["en"].Name.Should().Be("Drinks");
    }

    [Fact]
    public void Create_WithNullTranslations_DefaultsToEmptyDictionary()
    {
        var group = CatalogGroup.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Name", null, CatalogGroupType.Food, 1);

        group.Translations.Should().NotBeNull();
        group.Translations.Should().BeEmpty();
    }
}
