using Identity.TenantMembers.Features.DeleteTenantMember;

namespace Identity.UnitTests.TenantMembers.Features;

public class DeleteTenantMemberCommandValidatorTests : BaseUnitTest
{
    private readonly DeleteTenantMemberCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidId_PassesValidation()
    {
        var command = new DeleteTenantMemberCommand(Guid.CreateVersion7());
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyId_FailsValidation()
    {
        var command = new DeleteTenantMemberCommand(Guid.Empty);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Id is required");
    }
}
