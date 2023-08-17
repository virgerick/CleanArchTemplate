namespace CleanArchTemplate.Server.Endpoints.Models.GetModel;

using CleanArchTemplate.Application.Vehicles.Models.Queries;
using CleanArchTemplate.Application.Vehicles.Types.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetModelEndPoint
{
     public static IEndpointConventionBuilder MapGetModelEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetModelAsync)
        .WithName("GetModel")
        .WithDisplayName("Get Model");
        public static async ValueTask<ResultList<ModelResponse>> GetModelAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetModelQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => ResultList<ModelResponse>.Failure(error.GetMessages().ToList())
        );
    }
}