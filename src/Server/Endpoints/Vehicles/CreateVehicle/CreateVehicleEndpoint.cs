using CleanArchTemplate.Application.Vehicles.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Vehicles.CreateVehicle;
public static class CreateVehicleEndpoint
{
      public static IEndpointConventionBuilder MapCreateVehicleEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetVehicleAsync)
        .WithName("CreateVehicle")
        .WithDisplayName("Create a new Vehicle");
        public static async ValueTask<Result<Guid>> GetVehicleAsync(ISender Mediator,CreateEditVehicleRequest request,CancellationToken cancellationToken=default){
       (string plateNumber, Guid modelId, string color) = request;
        var query = new CreateVehicleCommand(plateNumber,modelId,color);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<Result<Guid>>(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}