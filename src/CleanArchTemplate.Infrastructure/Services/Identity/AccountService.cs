using System;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Account;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Microsoft.EntityFrameworkCore;
using CleanArchTemplate.Domain.Identity;

namespace CleanArchTemplate.Infrastructure.Services.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUploadService _uploadService;
    private readonly IStringLocalizer<AccountService> _localizer;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUploadService uploadService,
        IStringLocalizer<AccountService> localizer)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _uploadService = uploadService;
        _localizer = localizer;
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await this._userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return  Result.Failure(_localizer["User Not Found."]);
        }

        var identityResult = await this._userManager.ChangePasswordAsync(
            user,
            model.Password,
            model.NewPassword);
        var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
        return identityResult.Succeeded ?  Result.Success() :  Result.Failure(errors);
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, string userId)
    {
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return  Result.Failure(string.Format(_localizer["Phone number {0} is already used."], request.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null || userWithSameEmail.Id == userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return  Result.Failure(_localizer["User Not Found."]);
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (request.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ?  Result.Success() :  Result.Failure(errors);
        }
        else
        {
            return  Result.Failure(string.Format(_localizer["Email {0} is already used."], request.Email));
        }
    }

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return  Result<string>.Failure(_localizer["User Not Found"]);
        }
        return  Result<string>.Success(data: user.ProfilePictureDataUrl!);
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return  Result<string>.Failure(message: _localizer["User Not Found"]);
        var filePath = _uploadService.UploadAsync(request);
        user.ProfilePictureDataUrl = filePath;
        var identityResult = await _userManager.UpdateAsync(user);
        var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
        return identityResult.Succeeded ?  Result<string>.Success(data: filePath) :  Result<string>.Failure(errors);
    }
}
