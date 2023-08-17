namespace CleanArchTemplate.Server.Endpoints.Models.EditModel;

using CleanArchTemplate.Application.Vehicles.Models.Commands;
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class EditModelEndPoint
{
     public static IEndpointConventionBuilder MapEditModelEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditModelAsync)
        .WithName("EditModel")
        .WithDisplayName("Edit Model");

        public static async ValueTask<Result<Guid>> EditModelAsync(Guid Id, CreateEditModelRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditModelCommand(Id,request.Name, request.Year, request.BrandId, request.TypeId);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
