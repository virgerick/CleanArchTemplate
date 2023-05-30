namespace CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Vehicles.Queries;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetDeletedVehicleEndPoint
{
     public static IEndpointConventionBuilder MapGetDeletedVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/GetDeleted", GetDeletedVehicleAsync)
        .WithName("GetDeletedVehicle")
        .WithTags("Vehicle")
        .WithDisplayName("Get Deleted Vehicle");
        public static async ValueTask<ResultList<VehicleResponse>> GetDeletedVehicleAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetDeletedVehicleQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => ResultList<VehicleResponse>.Failure(error.GetMessages().ToList())
        );
    }
}