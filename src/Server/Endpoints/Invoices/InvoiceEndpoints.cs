


using CleanArchTemplate.Server.Endpoints.Invoices.Create;

namespace CleanArchTemplate.Server.Endpoints.Invoices;
public class InvoiceEndpoints : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup("/Invoice")
            .WithTags("Invoice");
        group.MapCreateInvoiceEndpoint();
        /*
        group.MapGetInvoiceEndpoint();
        group.MapGetInvoiceByIdEndpoint();
    
        group.MapEditInvoiceEndpoint();
        group.MapDeleteInvoiceEndpoint();*
        */
        return group;
    }
}