using System;
using CleanArchTemplate.Application.Vehicles.Types.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.VehicleTypes.GetVehicleTypeById;

public static class GetVehicleTypeByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetVehicleTypeByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetVehicleTypeAsync)
       .WithName("GetVehicleTypeById")
       .WithTags("VehicleType")
       .WithDisplayName("Get VehicleType by id");
    public static async ValueTask<Result<IdNameResponse<Guid>>> GetVehicleTypeAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetVehicleTypeByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<IdNameResponse<Guid>>.Failure(error.GetMessages().ToList())
        );
    }
}

