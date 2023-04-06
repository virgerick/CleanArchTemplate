using CleanArchTemplate.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationIdentityRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public string Description { get; set; }
    public string Group { get; set; }
    public virtual ApplicationIdentityRole? Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }

    public ApplicationIdentityRoleClaim() : base()
    {
    }

    public ApplicationIdentityRoleClaim(string roleClaimdescription = null, string roleClaimGroup = null) : base()
    {
        Description = roleClaimdescription;
        Group = roleClaimGroup;
    }
}