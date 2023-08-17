using System;
using CleanArchTemplate.Application.Services.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Services.GetById;

public static class GetServiceByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetServiceByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetServiceAsync)
       .WithName("GetServiceById")
       .WithDisplayName("Get Service by id");
    public static async ValueTask<Result<ServiceResponse>> GetServiceAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetServiceByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<ServiceResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

