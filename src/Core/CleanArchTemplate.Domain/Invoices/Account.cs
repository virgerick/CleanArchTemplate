namespace CleanArchTemplate.Domain.Invoices;

public class Account
{
    public int Id { get; set; }
    public string AccountName { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
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
