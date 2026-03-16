using Location.Locations.Features.CreateLocation;

namespace Location.UnitTests.Locations.Features;

public class CreateLocationCommandValidatorTests : BaseUnitTest
{
    private readonly CreateLocationCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateLocationCommand(
            Guid.CreateVersion7(), "Hotel Amsterdam", "hotel-amsterdam",
            "Hotel", "Dam 1", null, null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyTenantId_FailsValidation()
    {
        var command = new CreateLocationCommand(
            Guid.Empty, "Hotel", "hotel", "Hotel", null, null, null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "TenantId is required");
    }

    [Fact]
    public void Validate_WithEmptyName_FailsWithExpectedError()
    {
        var command = new CreateLocationCommand(
            Guid.CreateVersion7(), "", "hotel", "Hotel", null, null, null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void Validate_WithInvalidSlug_FailsValidation()
    {
        var command = new CreateLocationCommand(
            Guid.CreateVersion7(), "Hotel", "INVALID SLUG!", "Hotel", null, null, null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidType_FailsValidation()
    {
        var command = new CreateLocationCommand(
            Guid.CreateVersion7(), "Hotel", "hotel", "InvalidType", null, null, null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
