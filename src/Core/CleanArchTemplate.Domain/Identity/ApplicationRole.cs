using CleanArchTemplate.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationRole : IdentityRole, IAuditableEntity<string>
{
    public ApplicationRole() : base()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
    }

    public ApplicationRole(string roleName, string description) : base(roleName)
    {
        Description = description;
        RoleClaims = new HashSet<ApplicationRoleClaim>();
    }

    public string Description { get; set; }
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }
}
