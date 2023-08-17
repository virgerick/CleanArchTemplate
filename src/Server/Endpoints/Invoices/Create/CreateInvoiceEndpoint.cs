using Azure;
using CleanArchTemplate.Application.Invoices.Commands;
using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Results;
using MediatR;
using Response = CleanArchTemplate.Shared.Wrapper.Result<System.Guid>;

namespace CleanArchTemplate.Server.Endpoints.Invoices.Create;

public static class CreateInvoiceEndpoint
{
    public static IEndpointConventionBuilder MapCreateInvoiceEndpoint(this IEndpointRouteBuilder endpoint) => endpoint
        .MapPost("/", CreateInvoiceAsync)
        .WithName("CreateInvoice")
        .WithDisplayName("Create a new Invoice");

    private static async ValueTask<Response> CreateInvoiceAsync(ISender mediator, CreateInvoiceRequest request,CancellationToken cancellationToken=default){
        var command = new CreateInvoiceCommand(request.CustomerId,request.IssueDate,request.SaveAsDraft,request.Lines);
        return await mediator.Send(command, cancellationToken);
        
    } 
}