namespace CleanArchTemplate.Server.Endpoints.Vehicles.EditVehicle;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Vehicles.Commands;
using CleanArchTemplate.Shared.Requests.Vehicles;

public static class GetVehicleEndPoint
{
     public static IEndpointConventionBuilder MapEditVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditVehicleAsync)
        .WithName("EditVehicle")
        .WithTags("Vehicle")
        .WithDisplayName("Edit Vehicle");

        public static async ValueTask<Result<Guid>> EditVehicleAsync(Guid Id, CreateEditVehicleRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditVehicleCommand(Id,request.plateNumber,request.modelId,request.color);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
