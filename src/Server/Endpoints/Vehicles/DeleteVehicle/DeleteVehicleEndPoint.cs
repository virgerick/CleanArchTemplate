namespace CleanArchTemplate.Server.Endpoints.Vehicles.DeleteVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Vehicles.Commands;

public static class DeleteVehicleEndPoint
{
     public static IEndpointConventionBuilder MapDeleteVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteVehicleAsync)
        .WithName("DeleteVehicle")
        .WithDisplayName("Delete Vehicle");

        public static async ValueTask<Result> DeleteVehicleAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteVehicleCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
