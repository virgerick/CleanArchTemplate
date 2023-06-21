namespace CleanArchTemplate.Server.Endpoints.Services.GetService;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Services.Queries;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetRouteDefaultEndPoint
{
     public static IEndpointConventionBuilder MapGetServiceDefaultEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/Default", GetServiceDefaultAsync)
        .WithName("GetServiceDefault")
        .WithTags("Service")
        .WithDisplayName("Get Service Default");
        public static async ValueTask<Result<ServiceDefaultResponse>> GetServiceDefaultAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetServiceDefaultQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<ServiceDefaultResponse>.Failure(error.GetMessages().ToList())
        );
    }
}