namespace CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Vehicles.Queries;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetVehicleEndPoint
{
     public static IEndpointConventionBuilder MapGetVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetVehicleAsync)
        .WithName("GetVehicle")
        .WithDisplayName("Get Vehicle");
        public static async ValueTask<ResultList<VehicleResponse>> GetVehicleAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetVehicleQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<ResultList<VehicleResponse>>(
            response => response,
            error => ResultList<VehicleResponse>.Failure(error.GetMessages().ToList())
        );
    }
}
