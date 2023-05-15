using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace CleanArchTemplate.Domain.Invoices;
public record InvoiceId(int value);
public class Invoice
{
    public int Id { get; private set; }
    public DateTime IssueDate { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public ICollection<InvoiceLine> InvoiceLines { get; private set; }

    protected Invoice() { } // Constructor protegido para EF Core

    public static Invoice Create(DateTime issueDate, CustomerId customerId)
    {
        return new Invoice { IssueDate = issueDate, CustomerId = customerId };
    }
}
