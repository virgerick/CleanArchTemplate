using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CleanArchTemplate.Domain.Identity;

using System;
using CleanArchTemplate.Domain.Common;
public  class ApplicationUser : IdentityUser,IAuditableRootEntity<string>
{
    private ApplicationUser() { }
    #region Auditable root
    public DateTimeOffset CreatedAt { get; set; } = default!;
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }= default!;
    public bool Deleted { get; set; }= default!;
    public string? DeletedBy { get; set; }  = default!;
    public DateTimeOffset? DeletedAt { get; set; } = default!;
    #endregion
    public static OneOf<ApplicationUser,List<ValidationFailure>> Create(string username,string email,string phoneNumber)
    {
        var user = new ApplicationUser
        {
            UserName=username,
            NormalizedUserName=username.ToUpper(),
            Email=email,
            NormalizedEmail=email.ToUpper(),
            PhoneNumber=phoneNumber
        };
        var validationResult = new ApplicationUserValidator().Validate(user);
        if (!validationResult.IsValid) return validationResult.Errors;
        return user;
    }
}

file sealed class ApplicationUserValidator : AbstractValidator<ApplicationUser>
{
    public ApplicationUserValidator()
    {
        RuleFor(u => u.UserName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6);
        RuleFor(u => u.Email)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .EmailAddress();
    }
}
