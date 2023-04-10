using CleanArchTemplate.Domain.Common;

using Microsoft.AspNetCore.Identity;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public string Description { get; set; } = default!;
    public string Group { get; set; } = default!;
    public ApplicationRole? Role { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = "";
    public DateTime? ModifiedAt { get; set; } = default;
    public string? ModifiedBy { get; set; } = default;

    public ApplicationRoleClaim() : base()
    {
    }

    public ApplicationRoleClaim(string roleClaimdescription, string roleClaimGroup) : base()
    {
        Description = roleClaimdescription;
        Group = roleClaimGroup;
    }
}