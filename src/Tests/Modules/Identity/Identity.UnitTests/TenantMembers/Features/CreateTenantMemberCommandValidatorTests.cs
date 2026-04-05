using Identity.TenantMembers.Features.CreateTenantMember;

namespace Identity.UnitTests.TenantMembers.Features;

public class CreateTenantMemberCommandValidatorTests : BaseUnitTest
{
    private readonly CreateTenantMemberCommandValidator _validator = new();

    private static CreateTenantMemberCommand ValidCommand() =>
        new(Guid.CreateVersion7(), "user@example.com", "John Doe", "Staff", "SecurePass1!");

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var result = _validator.Validate(ValidCommand());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyTenantId_FailsValidation()
    {
        var command = ValidCommand() with { TenantId = Guid.Empty };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "TenantId is required");
    }

    [Fact]
    public void Validate_WithEmptyEmail_FailsValidation()
    {
        var command = ValidCommand() with { Email = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidEmail_FailsValidation()
    {
        var command = ValidCommand() with { Email = "not-an-email" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
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

    [Fact]
    public void Validate_WithEmptyPassword_FailsValidation()
    {
        var command = ValidCommand() with { Password = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithShortPassword_FailsValidation()
    {
        var command = ValidCommand() with { Password = "short" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Password is required and must be at least 8 characters");
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
