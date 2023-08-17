using System;
using CleanArchTemplate.Application.Drivers.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Drivers.GetById;

public static class GetDriverByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetDriverByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetDriverAsync)
       .WithName("GetDriverById")
       .WithDisplayName("Get Driver by id");
    public static async ValueTask<Result<DriverResponse>> GetDriverAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetDriverByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<DriverResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

