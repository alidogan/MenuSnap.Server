using ServiceUnit.ServiceUnits.Features.CreateServiceUnit;

namespace ServiceUnit.UnitTests.ServiceUnits.Features;

public class CreateServiceUnitCommandValidatorTests : BaseUnitTest
{
    private readonly CreateServiceUnitCommandValidator _validator = new();

    private static CreateServiceUnitCommand ValidCommand() =>
        new(Guid.CreateVersion7(), "Table 1", "T1", "Table", 4, "Terrace", null, 1, null);

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var result = _validator.Validate(ValidCommand());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyLocationId_FailsValidation()
    {
        var command = ValidCommand() with { LocationId = Guid.Empty };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "LocationId is required");
    }

    [Fact]
    public void Validate_WithEmptyName_FailsValidation()
    {
        var command = ValidCommand() with { Name = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void Validate_WithEmptyCode_FailsValidation()
    {
        var command = ValidCommand() with { Code = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Code is required");
    }

    [Fact]
    public void Validate_WithInvalidType_FailsValidation()
    {
        var command = ValidCommand() with { Type = "InvalidType" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Type must be a valid ServiceUnitType");
    }

    [Fact]
    public void Validate_WithInvalidStatus_FailsValidation()
    {
        var command = ValidCommand() with { Status = "NotAStatus" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Status must be a valid ServiceUnitStatus");
    }

    [Fact]
    public void Validate_WithZeroCapacity_FailsValidation()
    {
        var command = ValidCommand() with { Capacity = 0 };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Capacity must be greater than 0");
    }

    [Fact]
    public void Validate_WithNullCapacity_PassesValidation()
    {
        var command = ValidCommand() with { Capacity = null };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("Table")]
    [InlineData("Room")]
    [InlineData("Bungalow")]
    [InlineData("Pitch")]
    [InlineData("Seat")]
    [InlineData("Kiosk")]
    [InlineData("Other")]
    public void Validate_WithAllValidTypes_PassesValidation(string type)
    {
        var command = ValidCommand() with { Type = type };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
