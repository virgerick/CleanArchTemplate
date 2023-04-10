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

    public string Description { get; set; } = default!;
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    public DateTime CreatedAt { get; set; } = default!;
    public string CreatedBy { get; set; } = default!;
    public DateTime? ModifiedAt { get; set; } = default!;
    public string? ModifiedBy { get; set; } = default!;
}
