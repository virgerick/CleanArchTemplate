namespace CleanArchTemplate.Server.Endpoints.Routes.DeleteRoute;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Routes.Commands;

public static class DeleteRouteEndPoint
{
     public static IEndpointConventionBuilder MapDeleteRouteEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteRouteAsync)
        .WithName("DeleteRoute")
        .WithDisplayName("Delete Route");

        public static async ValueTask<Result> DeleteRouteAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteRouteCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
