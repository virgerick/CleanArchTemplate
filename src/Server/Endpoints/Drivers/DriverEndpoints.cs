

using CleanArchTemplate.Server.Endpoints.Drivers.Create;
using CleanArchTemplate.Server.Endpoints.Drivers.DeleteDriver;
using CleanArchTemplate.Server.Endpoints.Drivers.Edit;
using CleanArchTemplate.Server.Endpoints.Drivers.GetById;
using CleanArchTemplate.Server.Endpoints.Drivers.GetDriver;

namespace CleanArchTemplate.Server.Endpoints.Drivers;
public class DriverEndpoints : IMapEndpoint
{
    private const string EndPoint = "Drivers";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapGetDriverEndpoint();
        group.MapGetDriverByIdEndpoint();
        group.MapCreateDriverEndpoint();
        group.MapEditDriverEndpoint();
        group.MapDeleteDriverEndpoint();
        return group;
    }
}