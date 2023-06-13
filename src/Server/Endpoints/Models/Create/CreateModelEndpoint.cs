
using CleanArchTemplate.Application.Vehicles.Models.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchTemplate.Server.Endpoints.Models.Create;
public static class CreateModelEndpoint
{
      public static IEndpointConventionBuilder MapCreateModelEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetModelAsync)
        .WithName("CreateModel")
        .WithTags("Model")
    .WithDisplayName("Create a new Model");
        public static async ValueTask<Result<Guid>> GetModelAsync(ISender Mediator, CreateEditModelRequest request,CancellationToken cancellationToken=default){
        var query = new CreateModelCommand(request.Name,request.Year,request.BrandId,request.TypeId);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}