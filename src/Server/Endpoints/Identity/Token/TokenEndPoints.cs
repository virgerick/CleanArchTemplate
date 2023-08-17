using CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;
using CleanArchTemplate.Server.Endpoints.Identity.Token.RefreshToken;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token;

public class TokenEndPoints : IMapEndpoint
{
    private const string EndPoint = "token";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapGetTokenEndpoint();
        group.MapGetRefreshTokenEndpoint();
        return group;
    }
}
