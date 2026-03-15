using Tenant.Tenants.Features.CreateTenant;

namespace Tenant.UnitTests.Tenants.Features;

public class CreateTenantCommandValidatorTests : BaseUnitTest
{
    private readonly CreateTenantCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateTenantCommand("Acme Corp", "acme-corp", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyName_FailsWithExpectedError()
    {
        var command = new CreateTenantCommand("", "acme-corp", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void Validate_WithEmptySlug_FailsWithExpectedError()
    {
        var command = new CreateTenantCommand("Acme Corp", "", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Slug is required");
    }

    [Fact]
    public void Validate_WithInvalidSlugFormat_FailsWithExpectedError()
    {
        var command = new CreateTenantCommand("Acme Corp", "Acme Corp!", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Slug must be lowercase alphanumeric with hyphens");
    }

    [Fact]
    public void Validate_WithNullLogoUrl_PassesValidation()
    {
        var command = new CreateTenantCommand("Acme Corp", "acme-corp", null, false);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
