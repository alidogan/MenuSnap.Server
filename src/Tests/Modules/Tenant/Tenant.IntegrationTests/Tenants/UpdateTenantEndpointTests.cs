using Tenant.Tenants.Features.CreateTenant;
using Tenant.Tenants.Features.GetTenantById;
using Tenant.Tenants.Features.UpdateTenant;

namespace Tenant.IntegrationTests.Tenants;

public class UpdateTenantEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task UpdateTenant_WithValidRequest_ReturnsOk200()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Original Corp", "original-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        var updateRequest = new UpdateTenantRequest(created!.Id, "Updated Corp", "updated-corp", null, false);
        var response = await Client.PutAsJsonAsync("/tenants", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UpdateTenantResponse>();
        result!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTenant_VerifiesUpdatedValues()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Before Corp", "before-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        await Client.PutAsJsonAsync("/tenants",
            new UpdateTenantRequest(created!.Id, "After Corp", "after-corp", "https://logo.com/new.png", false));

        var getResponse = await Client.GetAsync($"/tenants/{created.Id}");
        var tenant = await getResponse.Content.ReadFromJsonAsync<GetTenantByIdResponse>();
        tenant!.Tenant.Name.Should().Be("After Corp");
        tenant.Tenant.Slug.Should().Be("after-corp");
        tenant.Tenant.LogoUrl.Should().Be("https://logo.com/new.png");
        tenant.Tenant.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateTenant_WithNonExistingId_ReturnsNotFound404()
    {
        var request = new UpdateTenantRequest(Guid.CreateVersion7(), "Ghost Corp", "ghost-corp", null, true);

        var response = await Client.PutAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTenant_WithEmptyName_ReturnsBadRequest400()
    {
        var createResponse = await Client.PostAsJsonAsync("/tenants",
            new CreateTenantRequest("Valid Corp", "valid-corp", null, true));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTenantResponse>();

        var request = new UpdateTenantRequest(created!.Id, "", "valid-corp", null, true);
        var response = await Client.PutAsJsonAsync("/tenants", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
