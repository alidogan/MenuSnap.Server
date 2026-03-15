using Tenant.Tenants.Features.CreateTenant;
using Tenant.Tenants.Features.GetTenants;

namespace Tenant.IntegrationTests.Tenants;

public class GetTenantsEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetTenants_ReturnsOk200WithPaginatedResult()
    {
        var response = await Client.GetAsync("/tenants?PageIndex=0&PageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantsResponse>();
        result.Should().NotBeNull();
        result!.Tenants.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTenants_AfterCreatingTenants_ReturnsCorrectCount()
    {
        await Client.PostAsJsonAsync("/tenants", new CreateTenantRequest("Alpha Corp", "alpha-corp", null, true));
        await Client.PostAsJsonAsync("/tenants", new CreateTenantRequest("Beta Corp", "beta-corp", null, true));

        var response = await Client.GetAsync("/tenants?PageIndex=0&PageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantsResponse>();
        result!.Tenants.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetTenants_WithPageSize1_ReturnsOneTenant()
    {
        await Client.PostAsJsonAsync("/tenants", new CreateTenantRequest("Paged Corp", "paged-corp", null, true));

        var response = await Client.GetAsync("/tenants?PageIndex=0&PageSize=1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantsResponse>();
        result!.Tenants.Data.Count().Should().Be(1);
    }
}
