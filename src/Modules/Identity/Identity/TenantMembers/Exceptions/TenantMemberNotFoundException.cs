using Shared.Exceptions;

namespace Identity.TenantMembers.Exceptions;

public class TenantMemberNotFoundException(Guid id)
    : NotFoundException($"TenantMember with id '{id}' was not found.");
