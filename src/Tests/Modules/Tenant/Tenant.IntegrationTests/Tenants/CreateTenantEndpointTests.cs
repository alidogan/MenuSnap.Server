using Tenant.Tenants.Features.CreateTenant;

namespace Tenant.IntegrationTests.Tenants;

public class CreateTenantEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task CreateTenant_WithValidRequest_ReturnsCreated201()
    {
        var request = new CreateTenantRequest("Acme Corp", "acme-corp", null, true);

        var response = await Client.PostAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTenant_WithEmptyName_ReturnsBadRequest400()
    {
        var request = new CreateTenantRequest("", "acme-corp", null, true);

        var response = await Client.PostAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenant_WithEmptySlug_ReturnsBadRequest400()
    {
        var request = new CreateTenantRequest("Acme Corp", "", null, true);

        var response = await Client.PostAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenant_WithInvalidSlug_ReturnsBadRequest400()
    {
        var request = new CreateTenantRequest("Acme Corp", "INVALID SLUG!", null, true);

        var response = await Client.PostAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenant_WithLogoUrl_ReturnsCreated201WithId()
    {
        var request = new CreateTenantRequest("Logo Corp", "logo-corp", "https://example.com/logo.png", true);

        var response = await Client.PostAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantResponse>();
        result!.Id.Should().NotBeEmpty();
    }
}
