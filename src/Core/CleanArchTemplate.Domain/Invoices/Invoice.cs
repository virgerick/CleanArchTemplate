using System;
using System.Diagnostics.Contracts;
using System.Net;
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using FluentValidation;
using FluentValidation.Results;
using OneOf;
namespace CleanArchTemplate.Domain.Invoices;
public record struct InvoiceId(int Value);
public class Invoice:AuditableRootEntity<InvoiceId>
{
    public DateTime IssueDate { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public ICollection<InvoiceLine> InvoiceLines { get; private set; }

    protected Invoice() { }
    public static OneOf<Invoice, List<ValidationFailure>> Create(DateTime issueDate, CustomerId customerId)
    {
        var validator = new InvoiceValidator();
        var invoice = new Invoice
        {
            IssueDate = issueDate,
            CustomerId = customerId
        };

        var validationResult = validator.Validate(invoice);

        return validationResult.Errors.Any() ? validationResult.Errors : invoice;
    }
    public void AddInvoiceLine(InvoiceLine invoiceLine)
    {
        var validator = new InvoiceLineValidator();
        var validationResult = validator.Validate(invoiceLine);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        InvoiceLines.Add(invoiceLine);
    }

    public decimal Total=>InvoiceLines.Sum(invoiceLine => invoiceLine.Total);
    
}
public class InvoiceValidator : AbstractValidator<Invoice>
{
    public InvoiceValidator()
    {
        RuleFor(invoice => invoice.IssueDate).NotNull();
        RuleFor(invoice => invoice.CustomerId).NotNull();
        RuleFor(invoice => invoice.InvoiceLines).NotNull();
    }
}
public class InvoiceLineValidator : AbstractValidator<InvoiceLine>
{
    public InvoiceLineValidator()
    {
        RuleFor(invoiceLine => invoiceLine.Quantity).NotNull();
        RuleFor(invoiceLine => invoiceLine.ServiceId).NotNull();
    }
}