namespace CleanArchTemplate.Domain.Invoices;

public class InvoiceLine
{
    public InvoiceLineId Id { get; private set; }
    public InvoiceId InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; }
    public ServiceId ServiceId { get; private set; }
    public Service Service { get; private set; }
    public int Quantity { get; private set; }

    public decimal TotalAmount => Service.Price * Quantity;

    protected InvoiceLine() { } // Constructor protegido para EF Core

    public static InvoiceLine Create(InvoiceLineId id, InvoiceId invoiceId, ServiceId serviceId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        return new InvoiceLine { Id = id, InvoiceId = invoiceId, ServiceId = serviceId, Quantity = quantity };
    }
}
