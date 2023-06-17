using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Wrapper;
using System.Security.Claims;
using CleanArchTemplate.Shared.Constants.Storage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Blazored.LocalStorage;
using CleanArchTemplate.Client.Services.LocalStorages;

namespace CleanArchTemplate.Client.Authentication;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly ITokenApiService _tokenApiService;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IStringLocalizer<AuthenticationManager> _localizer;

    public AuthenticationManager(
       ITokenApiService tokenApiService,
        IEncrytedLocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider,
        IStringLocalizer<AuthenticationManager> localizer)
    {
        _tokenApiService = tokenApiService;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
        _localizer = localizer;
    }

    public async Task<ClaimsPrincipal> CurrentUser()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User;
    }

    public async Task<IResult> Login(TokenRequest model)
    {
    
        var result = await _tokenApiService.GetAsync(model);
        if (!result.Succeeded)
        {
            return result;
        }
        var token = result.Data.Token;
        var refreshToken = result.Data.RefreshToken;
        var userImageURL = result.Data.UserImageURL;
        await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
        await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
        if (!string.IsNullOrEmpty(userImageURL))
        {
            await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
        }

        await ((BlazorStateProvider)_authenticationStateProvider).StateChangedAsync();

        return await Result.SuccessAsync();

    }

    public async Task<IResult> Logout()
    {
        await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
        ((BlazorStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        return await Result.SuccessAsync();
    }

    public async Task<string> RefreshToken()
    {
        var token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
        var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

        var result = await _tokenApiService.RefreshAsync(new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });


        if (!result.Succeeded)
        {
            throw new ApplicationException(_localizer["Something went wrong during the refresh token action"]);
        }
        token = result.Data.Token;
        refreshToken = result.Data.RefreshToken;
        await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
        await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
        return token;
    }

    public async Task<string> TryRefreshToken()
    {
        //check if token exists
        var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
        if (string.IsNullOrEmpty(availableToken)) return string.Empty;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        if (diff.TotalMinutes <= 1)
            return await RefreshToken();
        return string.Empty;
    }

    public async Task<string> TryForceRefreshToken()
    {
        return await RefreshToken();
    }
}