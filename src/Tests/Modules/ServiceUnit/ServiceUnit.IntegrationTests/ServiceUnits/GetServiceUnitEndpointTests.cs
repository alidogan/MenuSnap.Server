using ServiceUnit.Contracts.ServiceUnits.Dtos;
using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;
using ServiceUnit.ServiceUnits.Features.GetServiceUnitById;
using ServiceUnit.ServiceUnits.Features.GetServiceUnitsByLocation;

namespace ServiceUnit.IntegrationTests.ServiceUnits;

public class GetServiceUnitEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<(Guid Id, Guid LocationId)> CreateServiceUnitAsync(
        Guid? locationId = null, string code = "G-001")
    {
        var locId = locationId ?? Guid.CreateVersion7();
        var createRequest = new CreateServiceUnitRequest(
            locId, "Table 1", code, "Table", 4, "Terrace", null, 1, null);
        var createResponse = await Client.PostAsJsonAsync("/service-units", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateServiceUnitResponse>();
        return (created!.Id, locId);
    }

    [Fact]
    public async Task GetServiceUnitById_WithExistingId_ReturnsOk()
    {
        var (id, _) = await CreateServiceUnitAsync(code: "G-002");

        var response = await Client.GetAsync($"/service-units/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetServiceUnitByIdResponse>();
        result!.ServiceUnit.Id.Should().Be(id);
        result.ServiceUnit.Name.Should().Be("Table 1");
        result.ServiceUnit.Code.Should().Be("G-002");
    }

    [Fact]
    public async Task GetServiceUnitById_WithNonExistingId_ReturnsNotFound()
    {
        var response = await Client.GetAsync($"/service-units/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetServiceUnitsByLocation_WithExistingLocation_ReturnsServiceUnits()
    {
        var locationId = Guid.CreateVersion7();
        await CreateServiceUnitAsync(locationId, "G-003");
        await CreateServiceUnitAsync(locationId, "G-004");

        var response = await Client.GetAsync($"/locations/{locationId}/service-units");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetServiceUnitsByLocationResponse>();
        result!.ServiceUnits.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetServiceUnitsByLocation_WithNoUnits_ReturnsEmptyList()
    {
        var response = await Client.GetAsync($"/locations/{Guid.CreateVersion7()}/service-units");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetServiceUnitsByLocationResponse>();
        result!.ServiceUnits.Should().BeEmpty();
    }
}
