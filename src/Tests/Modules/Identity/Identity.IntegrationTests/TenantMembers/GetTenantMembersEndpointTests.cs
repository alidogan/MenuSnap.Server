using Identity.Contracts.TenantMembers.Dtos;
using Identity.TenantMembers.Features.CreateTenantMember;
using Identity.TenantMembers.Features.GetTenantMembers;

namespace Identity.IntegrationTests.TenantMembers;

public class GetTenantMembersEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<(Guid TenantId, CreateTenantMemberResponse Member)> CreateMemberAsync(
        Guid? tenantId = null, string email = "member@example.com")
    {
        var tid = tenantId ?? Guid.CreateVersion7();
        var request = new CreateTenantMemberRequest(
            tid, email, "Test Member", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantMemberResponse>();
        return (tid, result!);
    }

    [Fact]
    public async Task GetTenantMembers_WithExistingTenant_ReturnsOkWithMembers()
    {
        var (tenantId, _) = await CreateMemberAsync();

        var response = await Client.GetAsync($"/tenant-members?TenantId={tenantId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantMembersResponse>();
        result!.Members.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTenantMembers_WithNoMembers_ReturnsOkWithEmptyList()
    {
        var emptyTenantId = Guid.CreateVersion7();

        var response = await Client.GetAsync($"/tenant-members?TenantId={emptyTenantId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTenantMembersResponse>();
        result!.Members.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTenantMembers_ReturnsCorrectMemberData()
    {
        var (tenantId, _) = await CreateMemberAsync(email: "detail@example.com");

        var response = await Client.GetAsync($"/tenant-members?TenantId={tenantId}");

        var result = await response.Content.ReadFromJsonAsync<GetTenantMembersResponse>();
        var member = result!.Members.First();
        member.Email.Should().Be("detail@example.com");
        member.DisplayName.Should().Be("Test Member");
        member.Role.Should().Be("Staff");
        member.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetTenantMembers_OnlyReturnsMembersForRequestedTenant()
    {
        var tenantA = Guid.CreateVersion7();
        var tenantB = Guid.CreateVersion7();

        await CreateMemberAsync(tenantA, "a@example.com");
        await CreateMemberAsync(tenantB, "b@example.com");

        var response = await Client.GetAsync($"/tenant-members?TenantId={tenantA}");
        var result = await response.Content.ReadFromJsonAsync<GetTenantMembersResponse>();

        result!.Members.Should().AllSatisfy(m => m.TenantId.Should().Be(tenantA));
    }
}
