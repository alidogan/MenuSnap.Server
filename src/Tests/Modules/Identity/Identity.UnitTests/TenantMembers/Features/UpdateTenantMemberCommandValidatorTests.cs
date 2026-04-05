using Identity.TenantMembers.Features.UpdateTenantMember;

namespace Identity.UnitTests.TenantMembers.Features;

public class UpdateTenantMemberCommandValidatorTests : BaseUnitTest
{
    private readonly UpdateTenantMemberCommandValidator _validator = new();

    private static UpdateTenantMemberCommand ValidCommand() =>
        new(Guid.CreateVersion7(), "Jane Doe", "Admin", true);

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var result = _validator.Validate(ValidCommand());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyId_FailsValidation()
    {
        var command = ValidCommand() with { Id = Guid.Empty };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Id is required");
    }

    [Fact]
    public void Validate_WithEmptyDisplayName_FailsValidation()
    {
        var command = ValidCommand() with { DisplayName = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "DisplayName is required");
    }

    [Fact]
    public void Validate_WithInvalidRole_FailsValidation()
    {
        var command = ValidCommand() with { Role = "SuperAdmin" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Role must be a valid TenantRole (Owner, Admin, Manager, Staff)");
    }

    [Theory]
    [InlineData("Owner")]
    [InlineData("Admin")]
    [InlineData("Manager")]
    [InlineData("Staff")]
    public void Validate_WithAllValidRoles_PassesValidation(string role)
    {
        var command = ValidCommand() with { Role = role };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
