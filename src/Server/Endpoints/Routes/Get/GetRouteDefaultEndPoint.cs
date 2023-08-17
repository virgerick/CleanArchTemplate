namespace CleanArchTemplate.Server.Endpoints.Routes.GetRoute;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Routes.Queries;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetRouteDefaultEndPoint
{
     public static IEndpointConventionBuilder MapGetRouteDefaultEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/Default", GetRouteDefaultAsync)
        .WithName("GetRouteDefault")
        .WithDisplayName("Get Route Default");
        public static async ValueTask<Result<RouteDefaultResponse>> GetRouteDefaultAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetRouteDefaultQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<RouteDefaultResponse>.Failure(error.GetMessages().ToList())
        );
    }
}