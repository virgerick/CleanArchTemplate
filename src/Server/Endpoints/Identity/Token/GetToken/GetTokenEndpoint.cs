using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;

public class GetTokenEndpoint : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint) => endpoint.MapPost("/token", GetTokenAsync)
        .WithName("Token")
        .WithTags("Token")
        .WithDisplayName("GetToken");

    private async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, ITokenService tokenService) => await tokenService.LoginAsync(request);

}

