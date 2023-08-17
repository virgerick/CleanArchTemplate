namespace CleanArchTemplate.Server.Endpoints.Drivers.DeleteDriver;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Drivers.Commands;

public static class DeleteDriverEndPoint
{
     public static IEndpointConventionBuilder MapDeleteDriverEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteDriverAsync)
        .WithName("DeleteDriver")
        .WithDisplayName("Delete Driver");

        public static async ValueTask<Result> DeleteDriverAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteDriverCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
