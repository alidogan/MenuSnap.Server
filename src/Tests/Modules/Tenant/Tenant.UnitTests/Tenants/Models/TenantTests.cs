using Tenant.Tenants.Events;
using TenantModel = Tenant.Tenants.Models.Tenant;

namespace Tenant.UnitTests.Tenants.Models;

public class TenantTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsTenantWithCorrectProperties()
    {
        var tenant = TenantModel.Create(
            Guid.CreateVersion7(), "Acme Corp", "acme-corp", "https://example.com/logo.png", true);

        tenant.Name.Should().Be("Acme Corp");
        tenant.Slug.Should().Be("acme-corp");
        tenant.LogoUrl.Should().Be("https://example.com/logo.png");
        tenant.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithValidData_AddsTenantCreatedDomainEvent()
    {
        var tenant = TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, true);

        tenant.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TenantCreatedEvent>();
    }

    [Fact]
    public void Create_WithNullLogoUrl_Succeeds()
    {
        var tenant = TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, false);

        tenant.LogoUrl.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => TenantModel.Create(Guid.CreateVersion7(), "", "acme-corp", null, true);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsArgumentException()
    {
        var act = () => TenantModel.Create(Guid.CreateVersion7(), "   ", "acme-corp", null, true);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptySlug_ThrowsArgumentException()
    {
        var act = () => TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "", null, true);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesAllProperties()
    {
        var tenant = TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, true);

        tenant.Update("Updated Corp", "updated-corp", "https://new.com/logo.png", false);

        tenant.Name.Should().Be("Updated Corp");
        tenant.Slug.Should().Be("updated-corp");
        tenant.LogoUrl.Should().Be("https://new.com/logo.png");
        tenant.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_WithEmptyName_ThrowsArgumentException()
    {
        var tenant = TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, true);

        var act = () => tenant.Update("", "acme-corp", null, true);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithEmptySlug_ThrowsArgumentException()
    {
        var tenant = TenantModel.Create(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, true);

        var act = () => tenant.Update("Acme Corp", "", null, true);

        act.Should().Throw<ArgumentException>();
    }
}
