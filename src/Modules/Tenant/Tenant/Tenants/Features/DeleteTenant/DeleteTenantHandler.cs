using Tenant.Tenants.Exceptions;

namespace Tenant.Tenants.Features.DeleteTenant;

public record DeleteTenantCommand(Guid Id) : ICommand<DeleteTenantResult>;

public record DeleteTenantResult(bool IsSuccess);

internal class DeleteTenantHandler(TenantDbContext dbContext)
    : ICommandHandler<DeleteTenantCommand, DeleteTenantResult>
{
    public async Task<DeleteTenantResult> Handle(
        DeleteTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await dbContext.Tenants
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (tenant is null)
            throw new TenantNotFoundException(command.Id);

        tenant.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteTenantResult(true);
    }
}
