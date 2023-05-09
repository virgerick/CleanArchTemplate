using CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;
using CleanArchTemplate.Server.Endpoints.Identity.Token.RefreshToken;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token
{
    public class TokenEndPoints : IMapEndpoint
    {
        public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
        {
            var group = endpoint.MapGroup("token");
            group.MapGetTokenEndpoint();
            group.MapGetRefreshTokenEndpoint();
            return group;
        }
    }
}
