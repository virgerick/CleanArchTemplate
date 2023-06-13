namespace CleanArchTemplate.Server.Endpoints.VehicleTypes.EditVehicleType;

using CleanArchTemplate.Application.Vehicles.Types.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

public static class GetVehicleTypeEndPoint
{
     public static IEndpointConventionBuilder MapEditVehicleTypeEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditVehicleTypeAsync)
        .WithName("EditVehicleType")
        .WithTags("VehicleType")
        .WithDisplayName("Edit VehicleType");

        public static async ValueTask<Result<Guid>> EditVehicleTypeAsync(Guid Id, NameRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditVehicleTypeCommand(Id,request.Name);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
