using CleanArchTemplate.Domain.Common;

namespace CleanArchTemplate.Domain.Accounting;
public record struct AccountId(Guid Value);
public class Account : AuditableRootEntity<AccountId>
{
    private List<Transaction> _transactions=new();

    public string Name { get;private set; }
    public AccountType Type { get; private set; }
    public decimal Balance { get; set; }
    public IReadOnlyList<Transaction> Transactions { get => _transactions;  }
      public void Deposit(decimal amount)
    {
        Balance += amount;
        _transactions.Add(new Transaction(this, null, amount, "Deposit"));
    }

    public void Withdraw(decimal amount)
    {
        if (Balance < amount)
        {
            throw new ArgumentException("Insufficient balance.", nameof(amount));
        }

        Balance -= amount;
        _transactions.Add(new Transaction(null, this, amount, "Withdraw"));
    }
}
