using Identity.TenantMembers.Exceptions;
using Identity.TenantMembers.Models;

namespace Identity.TenantMembers.Features.UpdateTenantMember;

public record UpdateTenantMemberCommand(
    Guid Id,
    string DisplayName,
    string Role,
    bool IsActive)
    : ICommand<UpdateTenantMemberResult>;

public record UpdateTenantMemberResult(bool IsSuccess);

public class UpdateTenantMemberCommandValidator : AbstractValidator<UpdateTenantMemberCommand>
{
    public UpdateTenantMemberCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.DisplayName).NotEmpty().WithMessage("DisplayName is required").MaximumLength(200);
        RuleFor(x => x.Role).NotEmpty()
            .Must(r => Enum.TryParse<TenantRole>(r, true, out _))
            .WithMessage("Role must be a valid TenantRole (Owner, Admin, Manager, Staff)");
    }
}

internal class UpdateTenantMemberHandler(IdentityDbContext dbContext)
    : ICommandHandler<UpdateTenantMemberCommand, UpdateTenantMemberResult>
{
    public async Task<UpdateTenantMemberResult> Handle(
        UpdateTenantMemberCommand command, CancellationToken cancellationToken)
    {
        var member = await dbContext.TenantMembers
            .FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (member is null)
            throw new TenantMemberNotFoundException(command.Id);

        member.Update(
            command.DisplayName,
            Enum.Parse<TenantRole>(command.Role, ignoreCase: true),
            command.IsActive);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateTenantMemberResult(true);
    }
}
