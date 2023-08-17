using System;
using CleanArchTemplate.Application.Vehicles.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Vehicles.GetVehicleById;

public static class GetVehicleByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetVehicleByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetVehicleAsync)
       .WithName("GetVehicleById")
       .WithDisplayName("Get Vehicle by id");
    public static async ValueTask<Result<VehicleResponse>> GetVehicleAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetVehicleByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<VehicleResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

