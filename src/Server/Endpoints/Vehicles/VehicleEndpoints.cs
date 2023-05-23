using CleanArchTemplate.Server.Endpoints.Vehicles.CreateVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicle;

namespace CleanArchTemplate.Server.Endpoints.Vehicles;
public class VehicleEndpoints : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup("/Vehicle");
        group.MapGetVehicleEndpoint();
        group.MapCreateVehicleEndpoint();
        return group;
    }
}