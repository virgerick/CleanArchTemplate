namespace CleanArchTemplate.Server.Endpoints.Routes.EditRoute;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Routes.Commands;
using CleanArchTemplate.Shared.Requests.Routes;

public static class GetRouteEndPoint
{
     public static IEndpointConventionBuilder MapEditRouteEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditRouteAsync)
        .WithName("EditRoute")
        .WithTags("Route")
        .WithDisplayName("Edit Route");

        public static async ValueTask<Result<Guid>> EditRouteAsync(Guid Id, CreateEditRouteRequest request, ISender Mediator,CancellationToken cancellationToken=default){
            (string Origin, string Destination, float Distance, float EstimatedTime, decimal Amount)=request;
        var query = new EditRouteCommand(Id,Origin,Destination,Distance,EstimatedTime,Amount);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
