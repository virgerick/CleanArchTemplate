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
    public string Description { get;private set; }
    public decimal Price { get; private set; }
    public ServiceId ServiceId { get; set; }= ServiceId.Empty;
    public Service Service { get; set; }
    public RouteId RouteId { get; set; } = RouteId.Empty;
    public Route Route { get; set; }
     public decimal Total => Price * Quantity;
    protected InvoiceLine() { } 

    public static OneOf<InvoiceLine, List<ValidationFailure>> Create(InvoiceId invoiceId, ServiceId serviceId,string description,decimal price,
        int quantity = 1)
    {
        var validator = new InvoiceLineValidator();
        var invoiceLine = new InvoiceLine
        {
            Id=InvoiceLineId.NewId(),
            InvoiceId = invoiceId,
            ServiceId = serviceId,
            Description = description,
            Price = price,
            Quantity = quantity
        };

        var validationResult = validator.Validate(invoiceLine);

        return validationResult.Errors.Any() ? validationResult.Errors : invoiceLine;
    }
    public static OneOf<InvoiceLine, List<ValidationFailure>> Create(InvoiceId invoiceId, RouteId routeId,string description,decimal price,
        int quantity = 1)
    {
        var validator = new InvoiceLineValidator();
        var invoiceLine = new InvoiceLine
        {
            Id=InvoiceLineId.NewId(),
            InvoiceId = invoiceId,
            RouteId = routeId,
            Description = description,
            Price = price,
            Quantity = quantity
        };

        var validationResult = validator.Validate(invoiceLine);

        return validationResult.Errors.Any() ? validationResult.Errors : invoiceLine;
    }
}
