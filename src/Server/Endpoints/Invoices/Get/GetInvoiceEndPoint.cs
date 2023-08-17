
using CleanArchTemplate.Application.Invoices.Queries;
using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Responses.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Server.Endpoints.Invoices.Get;
using Response=ResultList<InvoiceResponse>;
public static class GetInvoiceEndPoint

{
    public static IEndpointConventionBuilder MapGetInvoiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint
        .MapGet("/", CreateInvoiceAsync)
        .WithName("GetInvoice")
        .WithDisplayName("GetInvoice");

    private static async ValueTask<Response> CreateInvoiceAsync(ISender mediator, CancellationToken cancellationToken=default){
        var command = new GetInvoiceQuery();
        return await mediator.Send(command, cancellationToken);
        
    } 
}