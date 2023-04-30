using CleanArchTemplate.Application.Common.Interfaces.Services.Identity;
using CleanArchTemplate.Shared.Requests.Identity;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Localization;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;

public class GetTokenEndpoint : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        return endpoint.MapPost("/token", async (TokenRequest request, ITokenService tokenService) => await tokenService.LoginAsync(request) )
            .WithName("Token")
           .WithTags("Token")
           .WithDisplayName("GetToken");
    }
}

