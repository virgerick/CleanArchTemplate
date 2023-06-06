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
     public decimal Total
    {
        get
        {
            // Calculate the total cost of the invoice line.
            decimal total = Service!.Amount * Quantity;
            return total;
        }
    }
    protected InvoiceLine() { } // Constructor protegido para EF Core

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
}
