using Identity.TenantMembers.Exceptions;

namespace Identity.TenantMembers.Features.DeleteTenantMember;

public record DeleteTenantMemberCommand(Guid Id) : ICommand<DeleteTenantMemberResult>;

public record DeleteTenantMemberResult(bool IsSuccess);

public class DeleteTenantMemberCommandValidator : AbstractValidator<DeleteTenantMemberCommand>
{
    public DeleteTenantMemberCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}

internal class DeleteTenantMemberHandler(IdentityDbContext dbContext)
    : ICommandHandler<DeleteTenantMemberCommand, DeleteTenantMemberResult>
{
    public async Task<DeleteTenantMemberResult> Handle(
        DeleteTenantMemberCommand command, CancellationToken cancellationToken)
    {
        var member = await dbContext.TenantMembers
            .FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (member is null)
            throw new TenantMemberNotFoundException(command.Id);

        member.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteTenantMemberResult(true);
    }
}
