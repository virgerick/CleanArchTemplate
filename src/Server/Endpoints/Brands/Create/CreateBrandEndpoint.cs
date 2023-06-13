
using CleanArchTemplate.Application.Vehicles.Brands.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchTemplate.Server.Endpoints.Brands.Create;
public static class CreateBrandEndpoint
{
      public static IEndpointConventionBuilder MapCreateBrandEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetBrandAsync)
        .WithName("CreateBrand")
        .WithTags("Brand")
    .WithDisplayName("Create a new Brand");
        public static async ValueTask<Result<Guid>> GetBrandAsync(ISender Mediator, CreateEditBrandRequest request,CancellationToken cancellationToken=default){
        var query = new CreateBrandCommand(request.Name,request.Logo);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}