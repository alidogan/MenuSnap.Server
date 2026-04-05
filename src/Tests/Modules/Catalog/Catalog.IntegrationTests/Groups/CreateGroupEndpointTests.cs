using Catalog.Groups.Features.CreateGroup;
using Catalog.Shared;

namespace Catalog.IntegrationTests.Groups;

public class CreateGroupEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task CreateGroup_WithValidRequest_ReturnsCreated201()
    {
        var request = new CreateGroupRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Breakfast", "Morning food", "Food", 1);

        var response = await Client.PostAsJsonAsync("/catalog/groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateGroupResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateGroup_WithEmptyName_ReturnsBadRequest()
    {
        var request = new CreateGroupRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "", null, "Food", 1);

        var response = await Client.PostAsJsonAsync("/catalog/groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGroup_WithInvalidType_ReturnsBadRequest()
    {
        var request = new CreateGroupRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Name", null, "NotAType", 1);

        var response = await Client.PostAsJsonAsync("/catalog/groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGroup_WithTranslations_ReturnsCreated201()
    {
        var translations = new Dictionary<string, LocalizedContent>
        {
            ["en"] = new("Breakfast", "Morning meals"),
            ["tr"] = new("Kahvaltı", "Sabah yemekleri")
        };

        var request = new CreateGroupRequest(
            Guid.CreateVersion7(), Guid.CreateVersion7(),
            "Ontbijt", "Ochtend maaltijden", "Food", 1,
            Translations: translations);

        var response = await Client.PostAsJsonAsync("/catalog/groups", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateGroupResponse>();
        result!.Id.Should().NotBeEmpty();
    }
}
