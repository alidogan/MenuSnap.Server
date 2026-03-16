using Catalog.Items.Features.GetCatalogByLocation;

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
}
