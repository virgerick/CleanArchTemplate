using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token.RefreshToken;

public static class RefreshTokenEndpoint 
{
    public static IEndpointConventionBuilder MapGetRefreshTokenEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/RefreshToken", GetRefreshTokenAsync)
        .WithName("RefreshToken")
        .WithDisplayName("Get Refresh Token");

    private static async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, ITokenService tokenService) => await tokenService.GetRefreshTokenAsync(request);

}

