namespace CleanArchTemplate.Server.Endpoints.Customers.GetCustomer;
using CleanArchTemplate.Shared.Extensions;
using CleanArchTemplate.Application.Customers.Queries;
using CleanArchTemplate.Shared.Responses.Customers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class GetCustomerEndPoint
{
     public static IEndpointConventionBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoint) => endpoint.MapGet("/", GetCustomerAsync)
        .WithDisplayName("Get Customer");
        public static async Task<ResultList<CustomerResponse>> GetCustomerAsync(ISender Mediator,CancellationToken cancellationToken=default){
        var query = new GetCustomerQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return  result.Match(
            response =>response,
             error => ResultList<CustomerResponse>.Failure(error.GetMessages().ToList())
        ) ;
    }
}