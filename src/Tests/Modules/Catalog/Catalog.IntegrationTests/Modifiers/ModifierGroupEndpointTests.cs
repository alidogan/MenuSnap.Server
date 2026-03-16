using Catalog.Groups.Features.CreateGroup;
using Catalog.Categories.Features.CreateCategory;
using Catalog.Items.Features.CreateItem;
using Catalog.Modifiers.Features.CreateModifierGroup;

namespace Catalog.IntegrationTests.Modifiers;

public class ModifierGroupEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<Guid> CreateItemAsync()
    {
        var tenantId = Guid.CreateVersion7();
        var locationId = Guid.CreateVersion7();

        var groupResp = await Client.PostAsJsonAsync("/catalog/groups",
            new CreateGroupRequest(tenantId, locationId, "Group", null, "Food", 1));
        var group = await groupResp.Content.ReadFromJsonAsync<CreateGroupResponse>();

        var catResp = await Client.PostAsJsonAsync("/catalog/categories",
            new CreateCategoryRequest(tenantId, group!.Id, "Category", null, 1));
        var cat = await catResp.Content.ReadFromJsonAsync<CreateCategoryResponse>();

        var itemResp = await Client.PostAsJsonAsync("/catalog/items",
            new CreateItemRequest(tenantId, cat!.Id, "Item", null, 5m, null, null, true, 1, null, null));
        var item = await itemResp.Content.ReadFromJsonAsync<CreateItemResponse>();

        return item!.Id;
    }

    [Fact]
    public async Task CreateModifierGroup_WithValidRequest_ReturnsCreated201()
    {
        var itemId = await CreateItemAsync();

        var request = new CreateModifierGroupRequest(
            Guid.CreateVersion7(), itemId,
            "Extra Toppings", false, true, null, null, 1);

        var response = await Client.PostAsJsonAsync("/catalog/modifier-groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateModifierGroupResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateModifierGroup_WithEmptyName_ReturnsBadRequest()
    {
        var request = new CreateModifierGroupRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "", false, false, null, null, 1);

        var response = await Client.PostAsJsonAsync("/catalog/modifier-groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteModifierGroup_WithNonExistingId_ReturnsNotFound()
    {
        var response = await Client.DeleteAsync($"/catalog/modifier-groups/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
