using Identity.Contracts.TenantMembers.Dtos;
using Identity.TenantMembers.Models;
using Shared.Contracts.CQRS;

namespace Identity.TenantMembers.Features.GetTenantMembers;

public record GetTenantMembersQuery(Guid TenantId) : IQuery<GetTenantMembersResult>;

public record GetTenantMembersResult(IEnumerable<TenantMemberDto> Members);

internal class GetTenantMembersHandler(IdentityDbContext dbContext)
    : IQueryHandler<GetTenantMembersQuery, GetTenantMembersResult>
{
    public async Task<GetTenantMembersResult> Handle(
        GetTenantMembersQuery query, CancellationToken cancellationToken)
    {
        var members = await dbContext.TenantMembers
            .Where(m => m.TenantId == query.TenantId)
            .OrderBy(m => m.DisplayName)
            .Select(m => new TenantMemberDto(
                m.Id, m.TenantId, m.UserId,
                m.Email, m.DisplayName,
                m.Role.ToString(),
                m.IsActive,
                m.CreatedAt, m.LastModified))
            .ToListAsync(cancellationToken);

        return new GetTenantMembersResult(members);
    }
}
