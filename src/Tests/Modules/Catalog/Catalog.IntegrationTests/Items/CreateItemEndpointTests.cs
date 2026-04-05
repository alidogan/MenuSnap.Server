using Catalog.Groups.Features.CreateGroup;
using Catalog.Categories.Features.CreateCategory;
using Catalog.Items.Features.CreateItem;
using Catalog.Shared;

namespace Catalog.IntegrationTests.Items;

public class CreateItemEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<Guid> CreateCategoryAsync()
    {
        var tenantId = Guid.CreateVersion7();
        var locationId = Guid.CreateVersion7();

        var groupResp = await Client.PostAsJsonAsync("/catalog/groups",
            new CreateGroupRequest(tenantId, locationId, "Group", null, "Food", 1));
        var group = await groupResp.Content.ReadFromJsonAsync<CreateGroupResponse>();

        var catResp = await Client.PostAsJsonAsync("/catalog/categories",
            new CreateCategoryRequest(tenantId, group!.Id, "Category", null, 1));
        var cat = await catResp.Content.ReadFromJsonAsync<CreateCategoryResponse>();

        return cat!.Id;
    }

    [Fact]
    public async Task CreateItem_WithValidRequest_ReturnsCreated201()
    {
        var categoryId = await CreateCategoryAsync();

        var request = new CreateItemRequest(
            Guid.CreateVersion7(), categoryId,
            "Scrambled Eggs", "Fluffy eggs", 8.50m,
            320, 10, true, 1,
            ["Eggs", "Milk"],
            ["Healthy"]);

        var response = await Client.PostAsJsonAsync("/catalog/items", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateItemResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateItem_WithNegativePrice_ReturnsBadRequest()
    {
        var request = new CreateItemRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Item", null, -1m, null, null, true, 1, null, null);

        var response = await Client.PostAsJsonAsync("/catalog/items", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateItem_WithEmptyName_ReturnsBadRequest()
    {
        var request = new CreateItemRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "", null, 5m, null, null, true, 1, null, null);

        var response = await Client.PostAsJsonAsync("/catalog/items", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateItem_WithTranslations_ReturnsCreated201()
    {
        var categoryId = await CreateCategoryAsync();
        var translations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Scrambled Eggs", "Fluffy eggs"),
            ["tr"] = new("Çırpılmış Yumurta", "Kabarık yumurta")
        };

        var request = new CreateItemRequest(
            Guid.CreateVersion7(), categoryId,
            "Roerei", "Luchtige eieren", 8.50m,
            320, 10, true, 1, ["Eggs"], ["Healthy"], translations);

        var response = await Client.PostAsJsonAsync("/catalog/items", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
