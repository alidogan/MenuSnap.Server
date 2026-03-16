using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;
using ServiceUnit.ServiceUnits.Features.UpdateServiceUnit;

namespace ServiceUnit.IntegrationTests.ServiceUnits;

public class UpdateServiceUnitEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<Guid> CreateServiceUnitAsync(string code = "U-001")
    {
        var createRequest = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "Table 1", code, "Table",
            4, "Terrace", null, 1, null);
        var createResponse = await Client.PostAsJsonAsync("/service-units", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateServiceUnitResponse>();
        return created!.Id;
    }

    [Fact]
    public async Task UpdateServiceUnit_WithValidRequest_ReturnsNoContent()
    {
        var id = await CreateServiceUnitAsync("U-002");

        var updateRequest = new UpdateServiceUnitRequest(
            "Room 1", "R-001", "Room", 2, "Indoor", null, 5, "Occupied", false);

        var response = await Client.PutAsJsonAsync($"/service-units/{id}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateServiceUnit_WithNonExistingId_ReturnsNotFound()
    {
        var updateRequest = new UpdateServiceUnitRequest(
            "Room 1", "R-002", "Room", null, null, null, 0, "Available", true);

        var response = await Client.PutAsJsonAsync($"/service-units/{Guid.CreateVersion7()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateServiceUnit_WithInvalidStatus_ReturnsBadRequest()
    {
        var id = await CreateServiceUnitAsync("U-003");

        var updateRequest = new UpdateServiceUnitRequest(
            "Room 1", "R-003", "Room", null, null, null, 0, "NotAStatus", true);

        var response = await Client.PutAsJsonAsync($"/service-units/{id}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
