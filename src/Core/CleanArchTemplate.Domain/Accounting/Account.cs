using CleanArchTemplate.Domain.Common;

namespace CleanArchTemplate.Domain.Invoices;
public record AccountId(Guid Value);
public class Account : AuditableRootEntity<AccountId>
{
    private List<Transaction> _transactions;

    public string AccountName { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    public IReadOnlyList<Transaction> Transactions { get => _transactions;  }
    public void Deposit(decimal amaunt)
    {
        Balance += amaunt;
    }
    public void Withdraw(decimal amount)
    {
        if (Balance < amount)
        {
            throw new ArgumentException("Insufficient balance.", nameof(amount));
        }

        Balance -= amount;
    }
}
