namespace CleanArchTemplate.Server.Endpoints.VehicleTypes.GetVehicleType;

using CleanArchTemplate.Application.Vehicles.Types.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetVehicleTypeEndPoint
{
     public static IEndpointConventionBuilder MapGetVehicleTypeEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetVehicleTypeAsync)
        .WithName("GetVehicleType")
        .WithTags("VehicleType")
        .WithDisplayName("Get VehicleType");
        public static async ValueTask<ResultList<IdNameResponse<Guid>>> GetVehicleTypeAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetVehicleTypeQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => ResultList<IdNameResponse<Guid>>.Failure(error.GetMessages().ToList())
        );
    }
}