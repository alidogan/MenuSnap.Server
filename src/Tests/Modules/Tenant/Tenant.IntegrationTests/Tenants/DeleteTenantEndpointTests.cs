using Tenant.Tenants.Features.CreateTenant;
using Tenant.Tenants.Features.DeleteTenant;

namespace Tenant.IntegrationTests.Tenants;

public class DeleteTenantEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task DeleteTenant_WithExistingId_ReturnsOk200()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Delete Corp", "delete-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        var response = await Client.DeleteAsync($"/tenants/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<DeleteTenantResponse>();
        result!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTenant_WithExistingId_TenantIsRemovedFromDb()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Gone Corp", "gone-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        await Client.DeleteAsync($"/tenants/{created!.Id}");

        var getResponse = await Client.GetAsync($"/tenants/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTenant_WithNonExistingId_ReturnsNotFound404()
    {
        var response = await Client.DeleteAsync($"/tenants/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
