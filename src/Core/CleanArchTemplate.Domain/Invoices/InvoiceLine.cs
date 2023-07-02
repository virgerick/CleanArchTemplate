using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using OneOf;
using FluentValidation.Results;

namespace CleanArchTemplate.Domain.Invoices;
public record struct InvoiceLineId(Guid Value) {
    public static InvoiceLineId NewId() => new(Guid.NewGuid());
};
public class InvoiceLine:AuditableEntity<InvoiceLineId>
{
    public InvoiceId InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; }
    public int Quantity { get; private set; } = 1;
    public ServiceId ServiceId { get; set; }
    public Service Service { get; set; }
    public RouteId RouteId { get; set; } = RouteId.Empty;
    public Route Route { get; set; }
     public decimal Total
    {
        get
        {
            if(ServiceId!=ServiceId.Empty)
            {
                ArgumentNullException.ThrowIfNull(Service, nameof(Service));
                return Service.Amount * Quantity;
            }
            if(RouteId!=RouteId.Empty)
            {
                ArgumentNullException.ThrowIfNull(Route, nameof(Route));
                return Route.Amount * Quantity;
            }
            return 0;
        }
    }
    protected InvoiceLine() { } 

    public static OneOf<InvoiceLine, List<ValidationFailure>> Create(InvoiceId invoiceId, ServiceId serviceId,
        int quantity = 1)
    {
        var validator = new InvoiceLineValidator();
        var invoiceLine = new InvoiceLine
        {
            InvoiceId = invoiceId,
            ServiceId = serviceId,
            Quantity = quantity
        };

        var validationResult = validator.Validate(invoiceLine);

        return validationResult.Errors.Any() ? validationResult.Errors : invoiceLine;
    }
    public static OneOf<InvoiceLine, List<ValidationFailure>> Create(InvoiceId invoiceId, RouteId routeId,
        int quantity = 1)
    {
        var validator = new InvoiceLineValidator();
        var invoiceLine = new InvoiceLine
        {
            InvoiceId = invoiceId,
            RouteId = routeId,
            Quantity = quantity
        };

        var validationResult = validator.Validate(invoiceLine);

        return validationResult.Errors.Any() ? validationResult.Errors : invoiceLine;
    }
}
