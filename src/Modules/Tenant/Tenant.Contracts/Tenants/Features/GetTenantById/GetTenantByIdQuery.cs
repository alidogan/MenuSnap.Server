using Shared.Contracts.CQRS;
using Tenant.Contracts.Tenants.Dtos;

namespace Tenant.Contracts.Tenants.Features.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IQuery<GetTenantByIdResult>;

public record GetTenantByIdResult(TenantDto Tenant);
