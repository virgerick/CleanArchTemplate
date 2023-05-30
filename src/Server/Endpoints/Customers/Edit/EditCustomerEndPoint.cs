namespace CleanArchTemplate.Server.Endpoints.Customers.Edit;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using CleanArchTemplate.Application.Customers.Commands;
using CleanArchTemplate.Shared.Requests.Customers;

public static class EditCustomerEndPoint
{
     public static IEndpointConventionBuilder MapEditCustomerEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPut("{Id}", EditCustomerAsync)
        .WithName("EditCustomer")
        .WithTags("Customer")
        .WithDisplayName("Edit Customer");

        public static async ValueTask<Result<Guid>> EditCustomerAsync(Guid Id, AddEditCustomerRequest request, ISender Mediator,CancellationToken cancellationToken=default){
        var query = new EditCustomerCommand(Id, request.Name, request.Email, request.Address.Street, request.Address.City, request.Address.State, request.Address.ZipCode);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => Result.Success(response),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}
