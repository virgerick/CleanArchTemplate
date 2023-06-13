using System;
using CleanArchTemplate.Application.Vehicles.Models.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Models.GetDefault
{
	public static class GetDefault
	{
        public static IEndpointConventionBuilder MapGetModelDefaultEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/GetDefault", GetModelAsync)
       .WithName("GetDefault")
       .WithTags("Model")
       .WithDisplayName("Get Model Default");
        public static async ValueTask<Result<ModelDefaultResponse>> GetModelAsync( ISender Mediator, CancellationToken cancellationToken = default)
        {
            var query = new GetModelDefaultQuery();
            var result = await Mediator.Send(query, cancellationToken);
            return result.Match(
                response => response,
                error => Result<ModelDefaultResponse>.Failure(error.GetMessages().ToList())
            );
        }
    }
}


