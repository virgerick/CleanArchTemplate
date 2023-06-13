using System;
using CleanArchTemplate.Application.Brands.Brands.Queries;
using CleanArchTemplate.Application.Vehicles.Types.Queries;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Brands.GetBrandById;

public static class GetBrandByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetBrandByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetBrandAsync)
       .WithName("GetBrandById")
       .WithTags("Brand")
       .WithDisplayName("Get Brand by id");
    public static async ValueTask<Result<BrandResponse>> GetBrandAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetBrandByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<BrandResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

