using Location.Locations.Features.CreateLocation;

namespace Location.IntegrationTests.Locations;

public class DeleteLocationEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task DeleteLocation_WithExistingId_ReturnsNoContent()
    {
        var createRequest = new CreateLocationRequest(
            Guid.CreateVersion7(), "To Delete", "to-delete-loc",
            "Restaurant", null, null, null);

        var createResponse = await Client.PostAsJsonAsync("/locations", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateLocationResponse>();

        var deleteResponse = await Client.DeleteAsync($"/locations/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteLocation_WithNonExistingId_ReturnsNotFound()
    {
        var response = await Client.DeleteAsync($"/locations/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
