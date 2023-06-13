namespace CleanArchTemplate.Server.Endpoints.Brands.EditBrand;

using CleanArchTemplate.Application.Vehicles.Brands.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetBrandEndPoint
{
     public static IEndpointConventionBuilder MapEditBrandEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditBrandAsync)
        .WithName("EditBrand")
        .WithTags("Brand")
        .WithDisplayName("Edit Brand");

        public static async ValueTask<Result<Guid>> EditBrandAsync(Guid Id, CreateEditBrandRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditBrandCommand(Id,request.Name,request.Logo);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
