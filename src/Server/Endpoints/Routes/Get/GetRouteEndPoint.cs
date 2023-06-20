namespace CleanArchTemplate.Server.Endpoints.Routes.GetRoute;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Routes.Queries;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetRouteEndPoint
{
     public static IEndpointConventionBuilder MapGetRouteEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetRouteAsync)
        .WithName("GetRoute")
        .WithTags("Route")
        .WithDisplayName("Get Route");
        public static async ValueTask<ResultList<RouteResponse>> GetRouteAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetRouteQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<ResultList<RouteResponse>>(
            response => response,
            error => ResultList<RouteResponse>.Failure(error.GetMessages().ToList())
        );
    }
}
