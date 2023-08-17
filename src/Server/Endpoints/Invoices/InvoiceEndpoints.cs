


using CleanArchTemplate.Server.Endpoints.Invoices.Create;
using CleanArchTemplate.Server.Endpoints.Invoices.Get;

namespace CleanArchTemplate.Server.Endpoints.Invoices;
public class InvoiceEndpoints : IMapEndpoint
{
    private const string EndPoint = "Invoices";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapCreateInvoiceEndpoint();
        group.MapGetInvoiceEndpoint();
          /*group.MapGetInvoiceByIdEndpoint();
    
        group.MapEditInvoiceEndpoint();
        group.MapDeleteInvoiceEndpoint();*
        */
        return group;
    }
}