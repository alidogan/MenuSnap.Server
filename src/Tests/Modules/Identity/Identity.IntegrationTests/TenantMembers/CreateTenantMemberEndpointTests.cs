using Identity.TenantMembers.Features.CreateTenantMember;

namespace Identity.IntegrationTests.TenantMembers;

public class CreateTenantMemberEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task CreateTenantMember_WithValidRequest_ReturnsCreated201()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), "newuser@example.com",
            "New User", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantMemberResponse>();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTenantMember_WithEmptyEmail_ReturnsBadRequest()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), "",
            "Test User", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenantMember_WithInvalidRole_ReturnsBadRequest()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), "user@example.com",
            "Test User", "SuperAdmin", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenantMember_WithShortPassword_ReturnsBadRequest()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), "user@example.com",
            "Test User", "Staff", "short");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTenantMember_WithEmptyDisplayName_ReturnsBadRequest()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), "user@example.com",
            "", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
