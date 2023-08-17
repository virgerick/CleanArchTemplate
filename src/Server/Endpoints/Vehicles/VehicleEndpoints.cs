using CleanArchTemplate.Server.Endpoints.Vehicles.ActivateVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.CreateVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.DeleteVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.EditVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicleById;
using CleanArchTemplate.Server.Endpoints.Vehicles.MaintenanceVehicle;
using CleanArchTemplate.Server.Endpoints.Vehicles.RestoreVehicle;

namespace CleanArchTemplate.Server.Endpoints.Vehicles;
public class VehicleEndpoints : IMapEndpoint
{
    private const string Endpoint = "Vehicles";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(Endpoint)
            .WithTags(Endpoint);
        group.MapGetVehicleEndpoint();
        group.MapGetVehicleDefaultEndpoint();
        group.MapGetDeletedVehicleEndpoint();
        group.MapGetVehicleByIdEndpoint();
        group.MapCreateVehicleEndpoint();
        group.MapEditVehicleEndpoint();
        group.MapDeleteVehicleEndpoint();
        group.MapRestoreVehicleEndpoint();
        group.MapMaintenanceVehicleEndpoint();
        group.MapActivateVehicleEndpoint();
        return group;
    }
}