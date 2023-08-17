using System;
using CleanArchTemplate.Application.Routes.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Routes.GetRouteById;

public static class GetRouteByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetRouteByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetRouteAsync)
       .WithName("GetRouteById")
       .WithDisplayName("Get Route by id");
    public static async ValueTask<Result<RouteResponse>> GetRouteAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<RouteResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

