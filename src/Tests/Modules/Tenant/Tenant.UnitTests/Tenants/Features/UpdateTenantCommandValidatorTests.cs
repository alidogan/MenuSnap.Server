using Tenant.Tenants.Features.UpdateTenant;

namespace Tenant.UnitTests.Tenants.Features;

public class UpdateTenantCommandValidatorTests : BaseUnitTest
{
    private readonly UpdateTenantCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var command = new UpdateTenantCommand(Guid.CreateVersion7(), "Acme Corp", "acme-corp", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyId_FailsWithExpectedError()
    {
        var command = new UpdateTenantCommand(Guid.Empty, "Acme Corp", "acme-corp", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Id is required");
    }

    [Fact]
    public void Validate_WithEmptyName_FailsWithExpectedError()
    {
        var command = new UpdateTenantCommand(Guid.CreateVersion7(), "", "acme-corp", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void Validate_WithEmptySlug_FailsWithExpectedError()
    {
        var command = new UpdateTenantCommand(Guid.CreateVersion7(), "Acme Corp", "", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Slug is required");
    }

    [Fact]
    public void Validate_WithInvalidSlugFormat_FailsWithExpectedError()
    {
        var command = new UpdateTenantCommand(Guid.CreateVersion7(), "Acme Corp", "UPPER CASE!", null, true);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Slug must be lowercase alphanumeric with hyphens");
    }
}
