namespace CleanArchTemplate.Server.Endpoints.Brands.GetBrand;

using CleanArchTemplate.Application.Vehicles.Brands.Queries;
using CleanArchTemplate.Application.Vehicles.Types.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetBrandEndPoint
{
     public static IEndpointConventionBuilder MapGetBrandEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetBrandAsync)
        .WithName("GetBrand")
        .WithDisplayName("Get Brand");
        public static async ValueTask<ResultList<BrandResponse>> GetBrandAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetBrandQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => ResultList<BrandResponse>.Failure(error.GetMessages().ToList())
        );
    }
}