using Location.Locations.Features.CreateLocation;

namespace Location.IntegrationTests.Locations;

public class CreateLocationEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task CreateLocation_WithValidRequest_ReturnsCreated201()
    {
        var request = new CreateLocationRequest(
            Guid.CreateVersion7(), "Hotel Amsterdam", "hotel-amsterdam-test",
            "Hotel", "Dam 1", "+31201234567", "Great hotel");

        var response = await Client.PostAsJsonAsync("/locations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateLocationResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateLocation_WithEmptyName_ReturnsBadRequest()
    {
        var request = new CreateLocationRequest(
            Guid.CreateVersion7(), "", "hotel", "Hotel", null, null, null);

        var response = await Client.PostAsJsonAsync("/locations", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateLocation_WithInvalidSlug_ReturnsBadRequest()
    {
        var request = new CreateLocationRequest(
            Guid.CreateVersion7(), "Hotel", "INVALID SLUG!", "Hotel", null, null, null);

        var response = await Client.PostAsJsonAsync("/locations", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateLocation_WithInvalidType_ReturnsBadRequest()
    {
        var request = new CreateLocationRequest(
            Guid.CreateVersion7(), "Hotel", "hotel", "NotAType", null, null, null);

        var response = await Client.PostAsJsonAsync("/locations", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
