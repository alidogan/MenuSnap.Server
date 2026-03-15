using Tenant.Tenants.Features.CreateTenant;
using Tenant.Tenants.Features.GetTenantById;

namespace Tenant.IntegrationTests.Tenants;

public class GetTenantByIdEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetTenantById_WithExistingId_ReturnsOk200WithTenant()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Find Corp", "find-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        var response = await Client.GetAsync($"/tenants/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantByIdResponse>();
        result!.Tenant.Id.Should().Be(created.Id);
        result.Tenant.Name.Should().Be("Find Corp");
        result.Tenant.Slug.Should().Be("find-corp");
    }

    [Fact]
    public async Task GetTenantById_WithNonExistingId_ReturnsNotFound404()
    {
        var response = await Client.GetAsync($"/tenants/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
