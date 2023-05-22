using System;
using System.Diagnostics.Contracts;
using System.Net;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;

namespace CleanArchTemplate.Domain.Invoices;
public record struct InvoiceId(int Value);
public class Invoice:AuditableRootEntity<InvoiceId>
{
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
