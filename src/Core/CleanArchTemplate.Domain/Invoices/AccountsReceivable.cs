namespace CleanArchTemplate.Domain.Invoices;

public class AccountsReceivable
{
    public AccountsReceivableId Id { get; private set; }
    public DateTime DueDate { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }

    protected AccountsReceivable() { } // Constructor protegido para EF Core

    public static AccountsReceivable Create(AccountsReceivableId id, DateTime dueDate, CustomerId customerId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        return new AccountsReceivable { Id = id, DueDate = dueDate, CustomerId = customerId, Amount = amount, IsPaid = false };
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}