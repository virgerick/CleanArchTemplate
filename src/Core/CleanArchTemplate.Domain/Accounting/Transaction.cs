using CleanArchTemplate.Domain.Common;

namespace CleanArchTemplate.Domain.Accounting;

public record struct TransactionId(Guid Value)
{
    public static TransactionId NewTransactionId() => new(Guid.NewGuid());
};
public class Transaction : AuditableRootEntity<TransactionId>
{
    private Transaction() { } // EF Core requires a private parameterless constructor

    public Transaction(Account originAccount, Account destinationAccount, decimal amount, string description)
    {
        Date = DateTime.UtcNow;
        Amount = amount;
        Description = description;

        OriginAccount = originAccount;
        DestinationAccount = destinationAccount;
    }

    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }

    public AccountId OriginAccountId { get; private set; }
    public Account OriginAccount { get; private set; }

    public AccountId DestinationAccountId { get; private set; }
    public Account DestinationAccount { get; private set; }
}