using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

namespace ServiceUnit.IntegrationTests.ServiceUnits;

public class CreateServiceUnitEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task CreateServiceUnit_WithValidRequest_ReturnsCreated201()
    {
        var request = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "Table 1", "T-001", "Table",
            4, "Terrace", null, 1, null);

        var response = await Client.PostAsJsonAsync("/service-units", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateServiceUnitResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateServiceUnit_WithEmptyName_ReturnsBadRequest()
    {
        var request = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "", "T-002", "Table",
            null, null, null, 1, null);

        var response = await Client.PostAsJsonAsync("/service-units", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateServiceUnit_WithEmptyCode_ReturnsBadRequest()
    {
        var request = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "Table 1", "", "Table",
            null, null, null, 1, null);

        var response = await Client.PostAsJsonAsync("/service-units", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateServiceUnit_WithInvalidType_ReturnsBadRequest()
    {
        var request = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "Table 1", "T-003", "NotAType",
            null, null, null, 1, null);

        var response = await Client.PostAsJsonAsync("/service-units", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
