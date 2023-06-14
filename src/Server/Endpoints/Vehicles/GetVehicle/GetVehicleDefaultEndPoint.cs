namespace CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Vehicles.Queries;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetVehicleDefaultEndPoint
{
     public static IEndpointConventionBuilder MapGetVehicleDefaultEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/Default", GetVehicleDefaultAsync)
        .WithName("GetVehicleDefault")
        .WithTags("Vehicle")
        .WithDisplayName("Get Vehicle Default");
        public static async ValueTask<Result<VehicleDefaultResponse>> GetVehicleDefaultAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetVehicleDefaultQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<VehicleDefaultResponse>.Failure(error.GetMessages().ToList())
        );
    }
}