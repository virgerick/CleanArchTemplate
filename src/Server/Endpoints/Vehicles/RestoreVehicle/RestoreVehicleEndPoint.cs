namespace CleanArchTemplate.Server.Endpoints.Vehicles.RestoreVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Vehicles.Commands;

public static class RestoreVehicleEndPoint
{
     public static IEndpointConventionBuilder MapRestoreVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPatch("/Restore/{Id}", RestoreVehicleAsync)
        .WithName("RestoreVehicle")
        .WithDisplayName("Restore Vehicle");

        public static async ValueTask<Result> RestoreVehicleAsync(Guid Id,  ISender Mediator,CancellationToken cancellationToken=default){
        var query = new RestoreVehicleCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
