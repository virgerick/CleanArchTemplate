namespace CleanArchTemplate.Domain.Invoices;

public  record InvoiceStatus
{
    public const string Draft = nameof(Draft);
    public const string Issued = nameof(Issued);
    public const string Paid = nameof(Paid);

    public string Status { get; }

    public InvoiceStatus(string status)
    {
        if (status != Draft && status != Issued && status != Paid)
        {
            throw new ArgumentException($"Invalid invoice status: {status}");
        }

        Status = status;
    }
}

public record DraftStatus() : InvoiceStatus(InvoiceStatus.Draft);
public record IssuedStatus() : InvoiceStatus(InvoiceStatus.Issued);
public record PaidStatus() : InvoiceStatus(InvoiceStatus.Paid);