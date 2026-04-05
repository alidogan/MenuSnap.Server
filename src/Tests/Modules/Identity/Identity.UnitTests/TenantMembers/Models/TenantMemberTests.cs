using Identity.TenantMembers.Models;

namespace Identity.UnitTests.TenantMembers.Models;

public class TenantMemberTests : BaseUnitTest
{
    private static TenantMember CreateValid(
        string email = "john@example.com",
        string displayName = "John Doe",
        TenantRole role = TenantRole.Staff) =>
        TenantMember.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            email, displayName, role);

    [Fact]
    public void Create_WithValidData_ReturnsMemberWithCorrectProperties()
    {
        var tenantId = Guid.CreateVersion7();
        var userId = Guid.CreateVersion7();

        var member = TenantMember.Create(
            Guid.CreateVersion7(), tenantId, userId,
            "alice@example.com", "Alice Smith", TenantRole.Admin, true);

        member.TenantId.Should().Be(tenantId);
        member.UserId.Should().Be(userId);
        member.Email.Should().Be("alice@example.com");
        member.DisplayName.Should().Be("Alice Smith");
        member.Role.Should().Be(TenantRole.Admin);
        member.IsActive.Should().BeTrue();
        member.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Create_WithEmptyEmail_ThrowsArgumentException()
    {
        var act = () => CreateValid(email: "");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyDisplayName_ThrowsArgumentException()
    {
        var act = () => CreateValid(displayName: "");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        var act = () => TenantMember.Create(
            Guid.CreateVersion7(), Guid.Empty, Guid.CreateVersion7(),
            "test@example.com", "Test", TenantRole.Staff);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithInactiveFlag_SetsIsActiveFalse()
    {
        var member = TenantMember.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(),
            "test@example.com", "Test User", TenantRole.Staff, isActive: false);

        member.IsActive.Should().BeFalse();
    }

    [Theory]
    [InlineData(TenantRole.Owner)]
    [InlineData(TenantRole.Admin)]
    [InlineData(TenantRole.Manager)]
    [InlineData(TenantRole.Staff)]
    public void Create_WithAllRoles_SetsRoleCorrectly(TenantRole role)
    {
        var member = CreateValid(role: role);
        member.Role.Should().Be(role);
    }

    [Fact]
    public void Update_WithValidData_UpdatesAllProperties()
    {
        var member = CreateValid();

        member.Update("Jane Doe", TenantRole.Manager, false);

        member.DisplayName.Should().Be("Jane Doe");
        member.Role.Should().Be(TenantRole.Manager);
        member.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_WithEmptyDisplayName_ThrowsArgumentException()
    {
        var member = CreateValid();
        var act = () => member.Update("", TenantRole.Staff, true);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_DoesNotChangeEmailOrTenantId()
    {
        var member = CreateValid(email: "original@example.com");
        var originalEmail = member.Email;
        var originalTenantId = member.TenantId;

        member.Update("New Name", TenantRole.Admin, true);

        member.Email.Should().Be(originalEmail);
        member.TenantId.Should().Be(originalTenantId);
    }

    [Fact]
    public void Delete_SetsIsDeletedToTrue()
    {
        var member = CreateValid();

        member.Delete();

        member.IsDeleted.Should().BeTrue();
    }
}
