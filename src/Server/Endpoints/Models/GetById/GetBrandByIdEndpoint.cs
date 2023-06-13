
using CleanArchTemplate.Application.Vehicles.Models.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Models.GetModelById;

public static class GetModelByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetModelByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetModelAsync)
       .WithName("GetModelById")
       .WithTags("Model")
       .WithDisplayName("Get Model by id");
    public static async ValueTask<Result<ModelResponse>> GetModelAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetModelByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<ModelResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

