namespace CleanArchTemplate.Server.Endpoints.Vehicles.MaintenanceVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Vehicles.Commands;

public static class MaintenanceVehicleEndPoint
{
     public static IEndpointConventionBuilder MapMaintenanceVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPatch("/Maintenance/{Id}", MaintenanceVehicleAsync)
        .WithName("MaintenanceVehicle")
        .WithDisplayName("Maintenance Vehicle");

        public static async ValueTask<Result> MaintenanceVehicleAsync(Guid Id,  ISender Mediator,CancellationToken cancellationToken=default){
        var query = new MaintenanceVehicleCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
