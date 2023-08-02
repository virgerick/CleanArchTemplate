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
            var(origin, destination, amount)=request;
        var query = new EditRouteCommand(Id,origin,destination,amount);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
