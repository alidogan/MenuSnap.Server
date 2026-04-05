using Catalog.Items.Models;
using Catalog.Shared;

namespace Catalog.UnitTests.Items;

public class CatalogItemTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsItemWithCorrectProperties()
    {
        var categoryId = Guid.CreateVersion7();

        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), categoryId,
            "Scrambled Eggs", "Fluffy eggs", 8.50m, 320, 10, true, 1,
            [Allergen.Eggs, Allergen.Milk],
            [ItemBadge.Healthy]);

        item.CategoryId.Should().Be(categoryId);
        item.Name.Should().Be("Scrambled Eggs");
        item.Price.Should().Be(8.50m);
        item.Allergens.Should().Contain(Allergen.Eggs);
        item.Badges.Should().Contain(ItemBadge.Healthy);
        item.IsAvailable.Should().BeTrue();
        item.Translations.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithNegativePrice_ThrowsArgumentException()
    {
        var act = () => CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Item", null, -1m, null, null, true, 1);

        act.Should().Throw<ArgumentException>().WithMessage("*negative*");
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "", null, 10m, null, null, true, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyCategoryId_ThrowsArgumentException()
    {
        var act = () => CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.Empty,
            "Name", null, 10m, null, null, true, 1);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Old", null, 5m, null, null, true, 1);

        item.Update("New", "Desc", 9.99m, 400, 15, false, 2, [Allergen.Gluten], [ItemBadge.New]);

        item.Name.Should().Be("New");
        item.Price.Should().Be(9.99m);
        item.IsAvailable.Should().BeFalse();
        item.Allergens.Should().Contain(Allergen.Gluten);
    }

    [Fact]
    public void SetImageUrl_WithValidUrl_SetsImageUrl()
    {
        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Name", null, 5m, null, null, true, 1);

        item.SetImageUrl("http://minio/photo.jpg");

        item.ImageUrl.Should().Be("http://minio/photo.jpg");
    }

    [Fact]
    public void Create_WithTranslations_StoresTranslationsCorrectly()
    {
        var translations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Scrambled Eggs", "Fluffy eggs"),
            ["tr"] = new("Çırpılmış Yumurta", "Kabarık yumurta")
        };

        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Roerei", "Luchtige eieren", 8.50m, 320, 10, true, 1,
            [Allergen.Eggs], [ItemBadge.Healthy], translations);

        item.Translations.Should().HaveCount(2);
        item.Translations["en"].Name.Should().Be("Scrambled Eggs");
        item.Translations["en"].Description.Should().Be("Fluffy eggs");
        item.Translations["tr"].Name.Should().Be("Çırpılmış Yumurta");
        item.Translations["tr"].Description.Should().Be("Kabarık yumurta");
    }

    [Fact]
    public void Update_WithTranslations_ReplacesTranslations()
    {
        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Old", null, 5m, null, null, true, 1,
            translations: new Dictionary<string, LocalizedContent>
            {
                ["en"] = new("Old English", null)
            });

        var newTranslations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("New English", "New desc"),
            ["tr"] = new("Yeni", "Yeni açıklama")
        };

        item.Update("New", "Desc", 9.99m, 400, 15, false, 2, null, null, newTranslations);

        item.Translations.Should().HaveCount(2);
        item.Translations["en"].Name.Should().Be("New English");
        item.Translations["tr"].Name.Should().Be("Yeni");
    }

    [Fact]
    public void Create_WithNullTranslations_DefaultsToEmptyDictionary()
    {
        var item = CatalogItem.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Name", null, 5m, null, null, true, 1);

        item.Translations.Should().NotBeNull();
        item.Translations.Should().BeEmpty();
    }
}
