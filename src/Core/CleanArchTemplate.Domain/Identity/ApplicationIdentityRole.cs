using CleanArchTemplate.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationIdentityRole : IdentityRole, IAuditableEntity<string>
{
    public ApplicationIdentityRole() : base()
    {
        RoleClaims = new HashSet<ApplicationIdentityRoleClaim>();
    }

    public ApplicationIdentityRole(string roleName, string description) : base(roleName)
    {
        Description = description;
        RoleClaims = new HashSet<ApplicationIdentityRoleClaim>();
    }

    public string Description { get; set; }
    public virtual ICollection<ApplicationIdentityRoleClaim> RoleClaims { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }
}
