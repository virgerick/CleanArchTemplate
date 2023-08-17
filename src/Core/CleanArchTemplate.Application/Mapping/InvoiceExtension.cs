using System.Linq.Expressions;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Invoices;

namespace CleanArchTemplate.Application.Mapping;

public static class InvoiceExtension
{
    public static InvoiceResponse Map(this Invoice x) => new InvoiceResponse
    { 
        Id =x.Id.Value,
        CustomerId =x.CustomerId.Value,
        Customer = x.Customer.Name??"",
        IssueDate =x.IssueDate,
        Status =x.Status.Status,
        Lines =x.Lines.Select(l=>l.Map()).ToList()
    };
    public static InvoiceLineResponse Map(this InvoiceLine x) => new InvoiceLineResponse()
    { 
        Id = x.Id.Value, 
        Description = x.Description,
        ServiceId = x.ServiceId.Value,
        Quantity = x.Quantity,
        RouteId = x.RouteId.Value,
        Price = x.Price
        
    };

    public static IQueryable<InvoiceResponse> ProjectTo(this IQueryable<Invoice> invoices)
        => invoices.Select(invoice => new InvoiceResponse
        {
            Id = invoice.Id.Value, 
            CustomerId =invoice.CustomerId.Value,
            Customer = invoice.Customer.Name??"",
            IssueDate =invoice.IssueDate,
            Status =invoice.Status.Status,
            Lines = invoice.Lines.Select(line=>new InvoiceLineResponse()
            { 
                Id = line.Id.Value, 
                Description = line.Description,
                ServiceId = line.ServiceId.Value,
                Quantity = line.Quantity,
                RouteId = line.RouteId.Value,
                Price = line.Price
        
            }).ToList()      
        });

    public static IQueryable<Invoice> Get(this IQueryable<Invoice> source,InvoiceId id=default, InvoiceStatus? status=null!,DateTime from=default,DateTime to=default, bool showDeleted=false)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (id != default)
        {
            source = source.Where(x => x.Id == id);
        }
        if (status != null)
        {
           source =  source.Where(x => x.Status == status);
        }

        if (from != default)
        {
            source = source.Where(x => x.IssueDate >= from);
        }

        if (to != default)
        {
            source = source.Where(x => x.IssueDate <= to);
        }

        if (showDeleted)
        {
            source = source.Where(x => !x.Deleted);
        }
        
        return source;
    }
}