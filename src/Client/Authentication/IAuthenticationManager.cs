using System;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Wrapper;
using System.Security.Claims;
namespace CleanArchTemplate.Client.Authentication;

public interface IAuthenticationManager
{
    Task<IResult> Login(TokenRequest model);

    Task<IResult> Logout();

    Task<string> RefreshToken();

    Task<string> TryRefreshToken();

    Task<string> TryForceRefreshToken();

    Task<ClaimsPrincipal> CurrentUser();
}
