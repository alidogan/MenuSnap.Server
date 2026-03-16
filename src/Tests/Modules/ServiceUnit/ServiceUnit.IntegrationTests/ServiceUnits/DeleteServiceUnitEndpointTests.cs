using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceUnit.Data;
using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

namespace ServiceUnit.IntegrationTests.ServiceUnits;

public class DeleteServiceUnitEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<Guid> CreateServiceUnitAsync(string code = "D-001")
    {
        var createRequest = new CreateServiceUnitRequest(
            Guid.CreateVersion7(), "Table 1", code, "Table",
            4, "Terrace", null, 1, null);
        var createResponse = await Client.PostAsJsonAsync("/service-units", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateServiceUnitResponse>();
        return created!.Id;
    }

    [Fact]
    public async Task DeleteServiceUnit_WithExistingId_ReturnsNoContent()
    {
        var id = await CreateServiceUnitAsync("D-002");

        var response = await Client.DeleteAsync($"/service-units/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteServiceUnit_WithNonExistingId_ReturnsNotFound()
    {
        var response = await Client.DeleteAsync($"/service-units/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteServiceUnit_SoftDeletesRecord_RecordStillExistsInDatabase()
    {
        var id = await CreateServiceUnitAsync("D-003");

        await Client.DeleteAsync($"/service-units/{id}");

        // Verify the record still exists in the DB but with IsDeleted = true
        var dbContext = Services.GetRequiredService<ServiceUnitDbContext>();
        var deletedUnit = await dbContext.ServiceUnits
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(s => s.Id == id);

        deletedUnit.Should().NotBeNull();
        deletedUnit!.IsDeleted.Should().BeTrue();
        deletedUnit.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteServiceUnit_AfterSoftDelete_GetByIdReturnsNotFound()
    {
        var id = await CreateServiceUnitAsync("D-004");

        await Client.DeleteAsync($"/service-units/{id}");

        var getResponse = await Client.GetAsync($"/service-units/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
