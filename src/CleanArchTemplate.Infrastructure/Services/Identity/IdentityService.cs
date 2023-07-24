using CleanArchTemplate.Application.Common.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using CleanArchTemplate.Domain.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchTemplate.Infrastructure.Services.Identity;

public class IdentityService : ITokenService
{
    private const string InvalidErrorMessage = "Invalid email or password.";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppConfiguration _appConfig;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IStringLocalizer<IdentityService> _localizer;
    private readonly IDateTimeService _dateTime;

    public IdentityService(
        UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        IOptions<AppConfiguration> appConfig, SignInManager<ApplicationUser> signInManager,
        IStringLocalizer<IdentityService> localizer, IDateTimeService dateTime)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appConfig = appConfig.Value;
        _signInManager = signInManager;
        _localizer = localizer;
        _dateTime = dateTime;
    }

    public  Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
    {
        return Result<TokenResponse>.TryCatch(async () => {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Result<TokenResponse>.Failure(_localizer["User Not Found."]);
            }
            if (!user.IsActive)
            {
                return Result<TokenResponse>.Failure(_localizer["User Not Active. Please contact the administrator."]);
            }
            if (!user.EmailConfirmed)
            {
                return Result<TokenResponse>.Failure(_localizer["E-Mail not confirmed."]);
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return Result<TokenResponse>.Failure(_localizer["Invalid Credentials."]);
            }
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = _dateTime.NowUtc.AddHours(7);
            await _userManager.UpdateAsync(user);
            var token = await GenerateJwtAsync(user);
            TokenResponse response = new TokenResponse { Token = token,
                RefreshToken = user.RefreshToken, 
                UserImageURL = user.ProfilePictureDataUrl,
                RefreshTokenExpiryTime=user.RefreshTokenExpiryTime
                
            };
            return Result<TokenResponse>.Success(response);
        });
    }

    public  Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
    {
        return Result<TokenResponse>.TryCatch(async () => {
            if (model is null)
            {
                return Result<TokenResponse>.Failure(_localizer["Invalid Client Token."]);
            }
            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail!);
            if (user == null)
                return Result<TokenResponse>.Failure(_localizer["User Not Found."]);
            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return Result<TokenResponse>.Failure(_localizer["Invalid Client Token."]);
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
            return Result<TokenResponse>.Success(response);
        });
    }

    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
        return token;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
            var thisRole = await _roleManager.FindByNameAsync(role);
            var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole!);
            permissionClaims.AddRange(allPermissionsForThisRoles);
        }

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddDays(2),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken!;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(_localizer["Invalid token"]);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_appConfig.Secret ?? "vbhvjnmvhvcbnfdbvvbnmcbnfvnbvmngjhgyuvmng78iy79uih789hjkbhjghjt678vjh67uguyt678tg");
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
