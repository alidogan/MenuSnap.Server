using Catalog.Groups.Features.CreateGroup;
using Catalog.Categories.Features.CreateCategory;
using Catalog.Items.Features.CreateItem;
using Catalog.Items.Features.GetCatalogByLocation;
using Catalog.Shared;

namespace Catalog.IntegrationTests.Items;

public class GetCatalogByLocationEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetCatalogByLocation_WithExistingLocation_ReturnsOk200()
    {
        var locationId = Guid.CreateVersion7();

        var response = await Client.GetAsync($"/catalog/location/{locationId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var catalog = await response.Content.ReadFromJsonAsync<CatalogLocationView>();
        catalog!.LocationId.Should().Be(locationId);
        catalog.Groups.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCatalogByLocation_IsPublic_NoAuthRequired()
    {
        var response = await Client.GetAsync($"/catalog/location/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCatalogByLocation_WithTranslatedItems_ReturnsTranslations()
    {
        var locationId = Guid.CreateVersion7();
        var tenantId = Guid.CreateVersion7();

        var groupTranslations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Breakfast", "Morning meals")
        };
        var groupResp = await Client.PostAsJsonAsync("/catalog/groups",
            new CreateGroupRequest(tenantId, locationId, "Ontbijt", "Ochtend maaltijden", "Food", 1,
                Translations: groupTranslations));
        var group = await groupResp.Content.ReadFromJsonAsync<CreateGroupResponse>();

        var categoryTranslations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Egg dishes", null)
        };
        var catResp = await Client.PostAsJsonAsync("/catalog/categories",
            new CreateCategoryRequest(tenantId, group!.Id, "Eiergerechten", null, 1,
                Translations: categoryTranslations));
        var cat = await catResp.Content.ReadFromJsonAsync<CreateCategoryResponse>();

        var itemTranslations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Scrambled Eggs", "Fluffy eggs"),
            ["tr"] = new("Çırpılmış Yumurta", "Kabarık yumurta")
        };
        await Client.PostAsJsonAsync("/catalog/items",
            new CreateItemRequest(tenantId, cat!.Id, "Roerei", "Luchtige eieren", 8.50m,
                320, 10, true, 1, ["Eggs"], ["Healthy"], itemTranslations));

        var response = await Client.GetAsync($"/catalog/location/{locationId}");
        var catalog = await response.Content.ReadFromJsonAsync<CatalogLocationView>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        catalog!.Groups.Should().HaveCount(1);
        catalog.Groups[0].Translations.Should().ContainKey("en");
        catalog.Groups[0].Translations["en"].Name.Should().Be("Breakfast");

        var category = catalog.Groups[0].Categories[0];
        category.Translations.Should().ContainKey("en");
        category.Translations["en"].Name.Should().Be("Egg dishes");

        var item = category.Items[0];
        item.Translations.Should().HaveCount(2);
        item.Translations["en"].Name.Should().Be("Scrambled Eggs");
        item.Translations["tr"].Name.Should().Be("Çırpılmış Yumurta");
    }
}
