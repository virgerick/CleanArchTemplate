using CleanArchTemplate.Domain.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationRole : IdentityRole, IAuditableEntity<string>
{
    private ApplicationRole() : base()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
    }
    public string Description { get; set; } = default!;
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    public DateTimeOffset CreatedAt { get ; set ; }
    public string CreatedBy { get ; set ; }
    public DateTimeOffset? ModifiedAt { get ; set ; }
    public string? ModifiedBy { get ; set ; }
    public static OneOf<ApplicationRole,IEnumerable<ValidationFailure>> Create(string name,string description)
    {
        var role = new ApplicationRole
        {
            Name=name,
            NormalizedName=name.ToUpper(),
            Description=description,
        };
        var validationResult = new ApplicationRoleValidator().Validate(role);
        if (!validationResult.IsValid) return validationResult.Errors;

        return role;
    }
}

file class ApplicationRoleValidator : AbstractValidator<ApplicationRole>
{
    public ApplicationRoleValidator()
    {
        RuleFor(r => r.Name)
            .NotNull().NotEmpty().MinimumLength(3);
        RuleFor(r => r.Description)
            .NotNull().NotEmpty();

    }
}