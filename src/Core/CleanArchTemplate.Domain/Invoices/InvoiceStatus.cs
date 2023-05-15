namespace CleanArchTemplate.Domain.Invoices;

public abstract record InvoiceStatus
{
    public static string Draft = nameof(Draft);
    public static string Issued = nameof(Issued);
    public static string Paid = nameof(Paid);

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
