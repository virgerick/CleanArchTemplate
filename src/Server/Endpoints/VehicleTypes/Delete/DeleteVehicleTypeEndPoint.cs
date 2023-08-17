namespace CleanArchTemplate.Server.Endpoints.VehicleTypes.DeleteVehicleType;

using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class DeleteVehicleTypeEndPoint
{
     public static IEndpointConventionBuilder MapDeleteVehicleTypeEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteVehicleTypeAsync)
        .WithName("DeleteVehicleType")
        .WithDisplayName("Delete VehicleType");

        public static async ValueTask<Result> DeleteVehicleTypeAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteVehicleTypeCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
