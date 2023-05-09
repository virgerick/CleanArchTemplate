using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;

public static class GetTokenEndpoint 
{
    public static IEndpointConventionBuilder MapGetTokenEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/GetToken", GetTokenAsync)
        .WithName("Token")
        .WithTags("Token")
        .WithDisplayName("Get Token");

    private static async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, ITokenService tokenService) => await tokenService.LoginAsync(request);

}
public static class RefreshTokenEndpoint 
{
    public static IEndpointConventionBuilder MapGetRefreshTokenEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/RefreshToken", GetRefreshTokenAsync)
        .WithName("RefreshToken")
        .WithTags("Token")
        .WithDisplayName("Get Refresh Token");

    private static async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, ITokenService tokenService) => await tokenService.GetRefreshTokenAsync(request);

}

