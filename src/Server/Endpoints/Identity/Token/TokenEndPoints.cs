using CleanArchTemplate.Server.Endpoints.Identity.Token.GetToken;

namespace CleanArchTemplate.Server.Endpoints.Identity.Token
{
    public class TokenEndPoints : IMapEndpoint
    {
        public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
        {
            var tokenGroup = endpoint.MapGroup("token");
            tokenGroup.MapGetTokenEndpoint();
            tokenGroup.MapGetRefreshTokenEndpoint();
            return tokenGroup;
        }
    }
}
