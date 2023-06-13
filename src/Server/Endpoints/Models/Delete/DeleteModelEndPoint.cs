namespace CleanArchTemplate.Server.Endpoints.Models.DeleteModel;

using CleanArchTemplate.Application.Vehicles.Models.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class DeleteModelEndPoint
{
     public static IEndpointConventionBuilder MapDeleteModelEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteModelAsync)
        .WithName("DeleteModel")
        .WithTags("Model")
        .WithDisplayName("Delete Model");

        public static async ValueTask<Result> DeleteModelAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteModelsCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
