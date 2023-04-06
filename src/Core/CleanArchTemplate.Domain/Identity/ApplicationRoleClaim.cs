using CleanArchTemplate.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public string Description { get; set; }
    public string Group { get; set; }
    public virtual ApplicationRole? Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }

    public ApplicationRoleClaim() : base()
    {
    }

    public ApplicationRoleClaim(string roleClaimdescription = null, string roleClaimGroup = null) : base()
    {
        Description = roleClaimdescription;
        Group = roleClaimGroup;
    }
}