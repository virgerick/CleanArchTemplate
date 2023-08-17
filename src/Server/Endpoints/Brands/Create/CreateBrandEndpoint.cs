
using CleanArchTemplate.Application.Vehicles.Brands.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Brands.Create;
public static class CreateBrandEndpoint
{
      public static IEndpointConventionBuilder MapCreateBrandEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetBrandAsync)
        .WithName("CreateBrand")
        .WithDisplayName("Create a new Brand");
      private static async ValueTask<Result<Guid>> GetBrandAsync(ISender mediator, CreateEditBrandRequest request,CancellationToken cancellationToken=default){
        var query = new CreateBrandCommand(request.Name,request.Logo);
        var result = await mediator.Send(query, cancellationToken);
        return result
            .Match(Result<Guid>.Success,
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
} 