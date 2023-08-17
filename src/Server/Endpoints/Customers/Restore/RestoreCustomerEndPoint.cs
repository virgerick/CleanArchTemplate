namespace CleanArchTemplate.Server.Endpoints.Customers.Restore;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Customers.Commands;

public static class RestoreCustomerEndPoint
{
     public static IEndpointConventionBuilder MapRestoreCustomerEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPatch("/Restore/{Id}", RestoreCustomerAsync)
        .WithName("RestoreCustomer")
        .WithDisplayName("Restore Customer");

        public static async ValueTask<Result> RestoreCustomerAsync(Guid Id,  ISender Mediator,CancellationToken cancellationToken=default){
        var query = new RestoreCustomerCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
