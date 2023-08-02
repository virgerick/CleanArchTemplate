using CleanArchTemplate.Application.Routes.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Routes.CreateRoute;
public static class CreateRouteEndpoint
{
      public static IEndpointConventionBuilder MapCreateRouteEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetRouteAsync)
        .WithName("CreateRoute")
        .WithTags("Route")
        .WithDisplayName("Create a new Route");
        public static async ValueTask<Result<Guid>> GetRouteAsync(ISender Mediator,CreateEditRouteRequest request,CancellationToken cancellationToken=default){
        var query = new CreateRouteCommand(request.Origin,request.Destination,request.Amount);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
