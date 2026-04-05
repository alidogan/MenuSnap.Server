using Identity.TenantMembers.Models;
using Identity.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.TenantMembers.Features.CreateTenantMember;

public record CreateTenantMemberCommand(
    Guid TenantId,
    string Email,
    string DisplayName,
    string Role,
    string Password,
    bool IsActive = true)
    : ICommand<CreateTenantMemberResult>;

public record CreateTenantMemberResult(Guid Id);

public class CreateTenantMemberCommandValidator : AbstractValidator<CreateTenantMemberCommand>
{
    public CreateTenantMemberCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required").MaximumLength(256);
        RuleFor(x => x.DisplayName).NotEmpty().WithMessage("DisplayName is required").MaximumLength(200);
        RuleFor(x => x.Role).NotEmpty()
            .Must(r => Enum.TryParse<TenantRole>(r, true, out _))
            .WithMessage("Role must be a valid TenantRole (Owner, Admin, Manager, Staff)");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password is required and must be at least 8 characters");
    }
}

internal class CreateTenantMemberHandler(
    IdentityDbContext dbContext,
    UserManager<ApplicationUser> userManager)
    : ICommandHandler<CreateTenantMemberCommand, CreateTenantMemberResult>
{
    public async Task<CreateTenantMemberResult> Handle(
        CreateTenantMemberCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        Guid userId;

        if (existingUser is not null)
        {
            userId = existingUser.Id;
        }
        else
        {
            var nameParts = command.DisplayName.Split(' ', 2);
            var newUser = new ApplicationUser
            {
                Id = Guid.CreateVersion7(),
                UserName = command.Email,
                Email = command.Email,
                DisplayName = command.DisplayName,
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(newUser, command.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            userId = newUser.Id;
        }

        var member = TenantMember.Create(
            Guid.CreateVersion7(),
            command.TenantId,
            userId,
            command.Email,
            command.DisplayName,
            Enum.Parse<TenantRole>(command.Role, ignoreCase: true),
            command.IsActive);

        dbContext.TenantMembers.Add(member);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateTenantMemberResult(member.Id);
    }
}
