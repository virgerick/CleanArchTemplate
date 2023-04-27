using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CleanArchTemplate.Domain.Identity;

using System;
using CleanArchTemplate.Domain.Common;
public  class ApplicationUser : IdentityUser,IAuditableRootEntity<string>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePictureDataUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
    public ApplicationUser() { }
    #region Auditable root
    public DateTimeOffset CreatedAt { get; set; } = default!;
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }= default!;
    public bool Deleted { get; set; }= default!;
    public string? DeletedBy { get; set; }  = default!;
    public DateTimeOffset? DeletedAt { get; set; } = default!;
    #endregion
    public static OneOf<ApplicationUser,List<ValidationFailure>> Create(string username,string email,string firstname,string lastname,string phoneNumber="",string ProfilePictureDataUrl="")
    {
        var user = new ApplicationUser
        {
            FirstName=firstname,
            LastName=lastname,
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
