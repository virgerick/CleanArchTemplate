
using CleanArchTemplate.Server.Endpoints.Routes.CreateRoute;
using CleanArchTemplate.Server.Endpoints.Routes.DeleteRoute;
using CleanArchTemplate.Server.Endpoints.Routes.EditRoute;
using CleanArchTemplate.Server.Endpoints.Routes.GetRoute;
using CleanArchTemplate.Server.Endpoints.Routes.GetRouteById;

namespace CleanArchTemplate.Server.Endpoints.Routes;
public class RouteEndpoints : IMapEndpoint
{
    private const string EndPoint = "Routes";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapGetRouteEndpoint();
        group.MapGetRouteDefaultEndpoint();
        group.MapGetRouteByIdEndpoint();
        group.MapCreateRouteEndpoint();
        group.MapEditRouteEndpoint();
        group.MapDeleteRouteEndpoint();
        return group;
    }
}