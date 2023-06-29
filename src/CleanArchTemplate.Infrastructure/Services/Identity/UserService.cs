using System.Text;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using CleanArchTemplate.Shared.Constants.Role;
using Microsoft.EntityFrameworkCore;
using CleanArchTemplate.Application.Common.Exceptions;
using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Shared.Requests.Mail;
using System.Globalization;
using System.Text.Encodings.Web;
using CleanArchTemplate.Domain.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq.Expressions;

namespace CleanArchTemplate.Infrastructure.Services.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMailService _mailService;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly IExcelService _excelService;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMailService mailService,
        IStringLocalizer<UserService> localizer,
        IExcelService excelService,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mailService = mailService;
        _localizer = localizer;
        _excelService = excelService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
       
        var users = await _userManager.Users
            .Select(MapResponse)
            .ToListAsync();
        return  Result<List<UserResponse>>.Success(users);
    }

    public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            return  Result.Failure(string.Format(_localizer["Username {0} is already taken."], request.UserName));
        }
        ApplicationUser user = null!;
        ApplicationUser
            .Create(request.UserName,request.Email,request.FirstName,request.LastName,request.PhoneNumber)
            .Switch(
                u=>user=u,
                _ => { }
            );

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return  Result.Failure(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.BasicRole);
                if (!request.AutoConfirmEmail)
                {
                    var verificationUri = await SendVerificationEmail(user, origin);
                    var mailRequest = new MailRequest
                    {
                        From = "mail@codewithmukesh.com",
                        To = user.Email!,
                        Body = string.Format(_localizer["Please confirm your account by <a href='{0}'>clicking here</a>."], verificationUri),
                        Subject = _localizer["Confirm Registration"]
                    };
                    //Todo: BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                    return  Result<string>.Success(user.Id, string.Format(_localizer["User {0} Registered. Please check your Mailbox to verify!"], user.UserName));
                }
                return  Result<string>.Success(user.Id, string.Format(_localizer["User {0} Registered."], user.UserName));
            }
            else
            {
                return  Result.Failure(result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
            }
        }
        else
        {
            return  Result.Failure(string.Format(_localizer["Email {0} is already registered."], request.Email));
        }
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/identity/user/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(MapResponse)
            .FirstOrDefaultAsync();
            if(user is null) throw new Exception("No found.");
        return  Result<UserResponse>.Success(user);
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
        var isAdmin = await _userManager.IsInRoleAsync(user!, RoleConstants.AdministratorRole);
        if (isAdmin)
        {
            return  Result.Failure(_localizer["Administrators Profile's Status cannot be toggled"]);
        }
        if (user != null)
        {
            user.IsActive = request.ActivateUser;
            var identityResult = await _userManager.UpdateAsync(user);
        }
        return  Result.Success();
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
    {
        var viewModel = new List<UserRoleModel>();
        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _roleManager.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var userRolesViewModel = new UserRoleModel
            {
                RoleName = role.Name!,
                RoleDescription = role.Description
            };
            if (await _userManager.IsInRoleAsync(user!, role.Name!))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            viewModel.Add(userRolesViewModel);
        }
        var result = new UserRolesResponse { UserRoles = viewModel };
        return  Result<UserRolesResponse>.Success(result);
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user!.Email == "mukesh@Application.com")
        {
            return  Result.Failure(_localizer["Not Allowed."]);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (!await _userManager.IsInRoleAsync(currentUser!, RoleConstants.AdministratorRole))
        {
            var tryToAddAdministratorRole = selectedRoles
                .Any(x => x.RoleName == RoleConstants.AdministratorRole);
            var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
            if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
            {
                return  Result.Failure(_localizer["Not Allowed to add or delete Administrator Role if you have not this role."]);
            }
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
        return  Result.Success(_localizer["Roles Updated"]);
    }

    public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user!, code);
        if (!result.Succeeded)
        {
            throw new ApiException(string.Format(_localizer["An error occurred while confirming {0}"], user!.Email));
        }
        return  Result<string>.Success(user!.Id, string.Format(_localizer["Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT."], user.Email));
        
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist or is not confirmed
            return  Result.Failure(_localizer["An Error has occurred!"]);
        }
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body = string.Format(_localizer["Please reset your password by <a href='{0}'>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL)),
            Subject = _localizer["Reset Password"],
            To = request.Email
        };
       //Todo: BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
        return  Result.Success(_localizer["Password Reset Mail has been sent to your authorized Email."]);
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return Result.Failure(_localizer["An Error has occured!"]);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            return Result.Failure(_localizer["An Error has occured!"]);
        }

        return Result.Success(_localizer["Password Reset Successful!"]);

    }

    public async Task<int> GetCountAsync()
    {
        var count = await _userManager.Users.CountAsync();
        return count;
    }

    public async Task<string> ExportToExcelAsync(string searchString = "")
    {
        var search = searchString.ToLower();
        var users = await _userManager.Users
            .Where(p =>string.IsNullOrEmpty(search) ? true : p.FirstName.Contains(search) || p.LastName.Contains(search) || p.Email!.Contains(search) || p.PhoneNumber!.Contains(search) || p.UserName!.Contains(search))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
        var result = await _excelService.ExportAsync(users, sheetName: _localizer["Users"],
            mappers: new Dictionary<string, Func<ApplicationUser, object>>
            {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["FirstName"], item => item.FirstName },
                    { _localizer["LastName"], item => item.LastName },
                    { _localizer["UserName"], item => item.UserName! },
                    { _localizer["Email"], item => item.Email! },
                    { _localizer["EmailConfirmed"], item => item.EmailConfirmed },
                    { _localizer["PhoneNumber"], item => item.PhoneNumber! },
                    { _localizer["PhoneNumberConfirmed"], item => item.PhoneNumberConfirmed },
                    { _localizer["IsActive"], item => item.IsActive },
                    { _localizer["CreatedOn (Local)"], item => item.CreatedAt.ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["CreatedOn (UTC)"], item => item.CreatedAt.ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["ProfilePictureDataUrl"], item => item.ProfilePictureDataUrl! },
            });

        return result!;
    }
    Expression<Func<ApplicationUser, UserResponse>> MapResponse = u => new UserResponse
    {
        Id = u.Id,
        UserName = u.UserName!,
        Email = u.Email!,
        EmailConfirmed = u.EmailConfirmed,
        FirstName = u.FirstName,
        LastName = u.LastName,
        PhoneNumber = u.PhoneNumber!,
        IsActive = u.IsActive,
        ProfilePictureDataUrl = u.ProfilePictureDataUrl!

    };
}