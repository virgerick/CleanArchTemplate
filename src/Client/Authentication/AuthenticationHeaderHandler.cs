﻿
using CleanArchTemplate.Shared.Constants.Permission;
using CleanArchTemplate.Shared.Constants.Storage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace CleanArchTemplate.Client.Authentication;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService localStorage;

    public AuthenticationHeaderHandler(ILocalStorageService localStorage)
        => this.localStorage = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var savedToken = await localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
public class BlazorStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    public ClaimsPrincipal AuthenticationStateUser { get; set; } = new ClaimsPrincipal(new ClaimsIdentity());
    public BlazorStateProvider(
        HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task StateChangedAsync()
    {
        var authState = Task.FromResult(await GetAuthenticationStateAsync());

        NotifyAuthenticationStateChanged(authState);

    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await this.GetAuthenticationStateAsync();
        var authenticationStateProviderUser = state.User;
        return authenticationStateProviderUser;
    }

   

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
        var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
        AuthenticationStateUser = state.User;
        return state;
    }

    private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
            if (permissions != null)
            {
                if (permissions.ToString().Trim().StartsWith("["))
                {
                    var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                    claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                }
                else
                {
                    claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                }
                keyValuePairs.Remove(ApplicationClaimTypes.Permission);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        }
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string payload)
    {
        payload = payload.Trim().Replace('-', '+').Replace('_', '/');
        var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
        return Convert.FromBase64String(base64);
    }
}