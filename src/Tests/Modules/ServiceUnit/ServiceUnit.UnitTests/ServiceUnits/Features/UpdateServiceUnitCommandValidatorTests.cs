using ServiceUnit.ServiceUnits.Features.UpdateServiceUnit;

namespace ServiceUnit.UnitTests.ServiceUnits.Features;

public class UpdateServiceUnitCommandValidatorTests : BaseUnitTest
{
    private readonly UpdateServiceUnitCommandValidator _validator = new();

    private static UpdateServiceUnitCommand ValidCommand() =>
        new(Guid.CreateVersion7(), "Table 1", "T1", "Table", 4, "Terrace", null, 1, "Available", true);

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
    public void Validate_WithEmptyStatus_FailsValidation()
    {
        var command = ValidCommand() with { Status = "" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Status is required");
    }

    [Fact]
    public void Validate_WithInvalidStatus_FailsValidation()
    {
        var command = ValidCommand() with { Status = "NotAStatus" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Status must be a valid ServiceUnitStatus");
    }

    [Theory]
    [InlineData("Available")]
    [InlineData("Occupied")]
    [InlineData("Reserved")]
    [InlineData("OutOfService")]
    [InlineData("Dirty")]
    [InlineData("Disabled")]
    public void Validate_WithAllValidStatuses_PassesValidation(string status)
    {
        var command = ValidCommand() with { Status = status };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
