namespace CleanArchTemplate.Server.Endpoints.Brands.DeleteBrand;

using CleanArchTemplate.Application.Vehicles.Brands.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class DeleteBrandEndPoint
{
     public static IEndpointConventionBuilder MapDeleteBrandEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteBrandAsync)
        .WithName("DeleteBrand")
        .WithDisplayName("Delete Brand");

        public static async ValueTask<Result> DeleteBrandAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteBrandsCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
