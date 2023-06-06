namespace CleanArchTemplate.Server.Endpoints.Services.DeleteService;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Services.Commands;

public static class DeleteServiceEndPoint
{
     public static IEndpointConventionBuilder MapDeleteServiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteServiceAsync)
        .WithName("DeleteService")
        .WithTags("Service")
        .WithDisplayName("Delete Service");

        public static async ValueTask<Result> DeleteServiceAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteServiceCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
