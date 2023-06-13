
using CleanArchTemplate.Server.Endpoints.VehicleTypes.Create;
using CleanArchTemplate.Server.Endpoints.VehicleTypes.DeleteVehicleType;
using CleanArchTemplate.Server.Endpoints.VehicleTypes.EditVehicleType;
using CleanArchTemplate.Server.Endpoints.VehicleTypes.GetVehicleType;
using CleanArchTemplate.Server.Endpoints.VehicleTypes.GetVehicleTypeById;

namespace CleanArchTemplate.Server.Endpoints.VehicleTypes;
public class VehicleTypeEndpoints : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup("/VehicleType")
            .WithTags("VehicleType");
        group.MapGetVehicleTypeEndpoint();
        group.MapGetVehicleTypeByIdEndpoint();
        group.MapCreateVehicleTypeEndpoint();
        group.MapEditVehicleTypeEndpoint();
        group.MapDeleteVehicleTypeEndpoint();
        return group;
    }
}