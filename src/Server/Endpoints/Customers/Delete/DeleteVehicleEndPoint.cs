namespace CleanArchTemplate.Server.Endpoints.Customers.DeleteCustomer;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Customers.Commands;

public static class DeleteCustomerEndPoint
{
     public static IEndpointConventionBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapDelete("{Id}", DeleteCustomerAsync)
        .WithName("DeleteCustomer")
        .WithTags("Customer")
        .WithDisplayName("Delete Customer");

        public static async ValueTask<Result> DeleteCustomerAsync(Guid Id, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new DeleteCustomerCommand(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result.Failure(error.GetMessages().ToList())
        );
    }
}
