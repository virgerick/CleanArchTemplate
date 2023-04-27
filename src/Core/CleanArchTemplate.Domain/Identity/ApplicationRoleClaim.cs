using System.Security.Claims;
using CleanArchTemplate.Domain.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CleanArchTemplate.Domain.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public string Description { get;private set; } = default!;
    public string Group { get;private set; } = default!;
    public ApplicationRole? Role { get;private set; } = default!;
    #region Auditable
    public DateTimeOffset CreatedAt { get ; set ; } = default!;
    public string CreatedBy { get ; set ; } = default!;
    public DateTimeOffset? ModifiedAt { get ; set ; } = default!;
    public string? ModifiedBy { get ; set ; } = default!;
    #endregion

    public ApplicationRoleClaim() : base()
    {
    }
    public static OneOf<ApplicationRoleClaim, IEnumerable<ValidationFailure>> Create(string claimType, string claimValue,string roleId="",string group="", string description="")
    {
        var claim = new ApplicationRoleClaim
        {
            RoleId = roleId,
            Group=group,
            Description = description,
            ClaimType=claimType,
            ClaimValue=claimValue
        };
        var validationResult = new ApplicationRoleClaimValidator().Validate(claim);
        if (!validationResult.IsValid) return validationResult.Errors;

        return claim;
    }

    public void Update(string roleId, string type, string value, string group, string description)
    {
        RoleId = roleId;
        Group = group;
        Description = description;
        ClaimType = type;
        ClaimValue = value;
    }
}

file class ApplicationRoleClaimValidator : AbstractValidator<ApplicationRoleClaim>
{
    public ApplicationRoleClaimValidator()
    {
        RuleFor(c => c.ClaimType)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2);
        RuleFor(c => c.ClaimValue)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2);
    }
}
