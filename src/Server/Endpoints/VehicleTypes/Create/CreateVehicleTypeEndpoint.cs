
using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.VehicleTypes.Create;
public static class CreateVehicleTypeEndpoint
{
      public static IEndpointConventionBuilder MapCreateVehicleTypeEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", GetVehicleTypeAsync)
        .WithName("CreateVehicleType")
        .WithTags("VehicleType")
        .WithDisplayName("Create a new VehicleType");
        public static async ValueTask<Result<Guid>> GetVehicleTypeAsync(ISender Mediator,NameRequest request,CancellationToken cancellationToken=default){
        var query = new CreateVehicleTypeCommand(request.Name);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}