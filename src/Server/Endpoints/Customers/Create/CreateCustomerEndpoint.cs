using CleanArchTemplate.Application.Customers.Commands;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Requests.Customers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Customers.Create;
public static class CreateCustomerEndpoint
{
      public static IEndpointConventionBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapPost("/", CreateAsync)
        .WithName("CreateCustomer")
        .WithDisplayName("Create a new Customer");
        public static async ValueTask<Result<Guid>> CreateAsync(ISender Mediator, AddEditCustomerRequest request,CancellationToken cancellationToken=default){
       
        var query = new CreateCustomerCommand(request.Name,request.Email,request.Address.Street,request.Address.City,request.Address.State,request.Address.ZipCode);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<Result<Guid>>(
            id => Result<Guid>.Success(id),
            error => Result<Guid>.Failure(error.GetMessages().ToList())
        );
    }
}