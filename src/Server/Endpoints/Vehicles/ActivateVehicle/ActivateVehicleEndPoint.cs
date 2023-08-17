namespace CleanArchTemplate.Server.Endpoints.Vehicles.ActivateVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Vehicles.Commands;

public static class ActivateVehicleEndPoint
{
     public static IEndpointConventionBuilder MapActivateVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPatch("/Activate/{Id}", ActivateVehicleAsync)
        .WithName("ActivateVehicle")
        .WithDisplayName("Activate Vehicle");

        public static async ValueTask<Result> ActivateVehicleAsync(Guid Id,  ISender Mediator,CancellationToken cancellationToken=default){
        var query = new ActivateVehicleCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
