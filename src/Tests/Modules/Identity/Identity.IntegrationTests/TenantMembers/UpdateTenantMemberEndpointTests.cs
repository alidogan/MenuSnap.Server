using Identity.TenantMembers.Features.CreateTenantMember;
using Identity.TenantMembers.Features.UpdateTenantMember;

namespace Identity.IntegrationTests.TenantMembers;

public class UpdateTenantMemberEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<Guid> CreateMemberAsync()
    {
        var request = new CreateTenantMemberRequest(
            Guid.CreateVersion7(), $"update-{Guid.CreateVersion7()}@example.com",
            "Original Name", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantMemberResponse>();
        return result!.Id;
    }

    [Fact]
    public async Task UpdateTenantMember_WithValidRequest_ReturnsOk()
    {
        var memberId = await CreateMemberAsync();
        var updateRequest = new UpdateTenantMemberRequest(
            memberId, "Updated Name", "Admin", true);

        var response = await Client.PutAsJsonAsync("/tenant-members", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UpdateTenantMemberResponse>();
        result!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTenantMember_WithNonExistingId_ReturnsNotFound()
    {
        var updateRequest = new UpdateTenantMemberRequest(
            Guid.CreateVersion7(), "Name", "Staff", true);

        var response = await Client.PutAsJsonAsync("/tenant-members", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTenantMember_WithEmptyDisplayName_ReturnsBadRequest()
    {
        var memberId = await CreateMemberAsync();
        var updateRequest = new UpdateTenantMemberRequest(
            memberId, "", "Staff", true);

        var response = await Client.PutAsJsonAsync("/tenant-members", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTenantMember_WithInvalidRole_ReturnsBadRequest()
    {
        var memberId = await CreateMemberAsync();
        var updateRequest = new UpdateTenantMemberRequest(
            memberId, "Name", "InvalidRole", true);

        var response = await Client.PutAsJsonAsync("/tenant-members", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
