using Identity.TenantMembers.Features.CreateTenantMember;
using Identity.TenantMembers.Features.GetTenantMembers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Data;

namespace Identity.IntegrationTests.TenantMembers;

public class DeleteTenantMemberEndpointTests(MenuSnapWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private async Task<(Guid MemberId, Guid TenantId)> CreateMemberAsync()
    {
        var tenantId = Guid.CreateVersion7();
        var request = new CreateTenantMemberRequest(
            tenantId, $"delete-{Guid.CreateVersion7()}@example.com",
            "To Delete", "Staff", "SecurePass1!");

        var response = await Client.PostAsJsonAsync("/tenant-members", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateTenantMemberResponse>();
        return (result!.Id, tenantId);
    }

    [Fact]
    public async Task DeleteTenantMember_WithExistingId_ReturnsOk()
    {
        var (memberId, _) = await CreateMemberAsync();

        var response = await Client.DeleteAsync($"/tenant-members/{memberId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteTenantMember_WithNonExistingId_ReturnsNotFound()
    {
        var response = await Client.DeleteAsync($"/tenant-members/{Guid.CreateVersion7()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTenantMember_IsSoftDelete_MemberNotReturnedInGetQuery()
    {
        var (memberId, tenantId) = await CreateMemberAsync();

        await Client.DeleteAsync($"/tenant-members/{memberId}");

        var getResponse = await Client.GetAsync($"/tenant-members?TenantId={tenantId}");
        var result = await getResponse.Content.ReadFromJsonAsync<GetTenantMembersResponse>();
        result!.Members.Should().NotContain(m => m.Id == memberId);
    }

    [Fact]
    public async Task DeleteTenantMember_IsSoftDelete_RecordStillExistsInDatabase()
    {
        var (memberId, _) = await CreateMemberAsync();

        await Client.DeleteAsync($"/tenant-members/{memberId}");

        var dbContext = Services.GetRequiredService<IdentityDbContext>();
        var member = await dbContext.TenantMembers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.Id == memberId);
        member.Should().NotBeNull();
        member!.IsDeleted.Should().BeTrue();
    }
}
