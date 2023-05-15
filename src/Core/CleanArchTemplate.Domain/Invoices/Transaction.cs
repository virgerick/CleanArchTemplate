namespace CleanArchTemplate.Domain.Invoices;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public float Amount { get; set; }

    public int OriginAccountId { get; set; }
    public Account OriginAccount { get; set; }

    public int DestinationAccountId { get; set; }
    public Account DestinationAccount { get; set; }
}
