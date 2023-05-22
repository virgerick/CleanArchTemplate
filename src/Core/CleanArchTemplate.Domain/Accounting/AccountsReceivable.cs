using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using System;
namespace CleanArchTemplate.Domain.Accounting;

public record AccountReceivableId(Guid Value);
public class AccountReceivable:AuditableRootEntity<AccountReceivableId>
{
    public DateTime DueDate { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }

    protected AccountReceivable() { } // Constructor protegido para EF Core

    public static AccountReceivable Create(AccountReceivableId id, DateTime dueDate, CustomerId customerId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        return new AccountReceivable { Id = id, DueDate = dueDate, CustomerId = customerId, Amount = amount, IsPaid = false };
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
        
    }
}