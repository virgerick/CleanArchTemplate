using System;
using CleanArchTemplate.Application.Customers.Queries;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Shared.Responses.Customers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Customers.GetById;

public static class GetCustomerByIdEndpoint
{
    public static IEndpointConventionBuilder MapGetCustomerByIdEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/{Id}", GetCustomerAsync)
       .WithName("GetCustomerById")
       .WithDisplayName("Get Customer by id");
    public static async ValueTask<Result<CustomerResponse>> GetCustomerAsync(Guid Id,ISender Mediator, CancellationToken cancellationToken = default)
    {
        var query = new GetCustomerByIdQuery(Id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match(
            response => response,
            error => Result<CustomerResponse>.Failure(error.GetMessages().ToList())
        );
    }
}

