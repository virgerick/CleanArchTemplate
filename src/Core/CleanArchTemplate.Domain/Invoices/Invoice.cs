
using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using FluentValidation;
using FluentValidation.Results;
using OneOf;
namespace CleanArchTemplate.Domain.Invoices;
public record struct InvoiceId(Guid Value);
public class Invoice:AuditableRootEntity<InvoiceId>
{
    public DateTime IssueDate { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get;  set; }
    public InvoiceStatus Status { get; private set; } = new DraftStatus();
    public ICollection<InvoiceLine> InvoiceLines { get;  set; }

    protected Invoice() { }
    public static OneOf<Invoice, List<ValidationFailure>> Create(DateTime issueDate, CustomerId customerId,InvoiceStatus? status=null!)
    {
        var validator = new InvoiceValidator();
        var invoice = new Invoice
        {
            IssueDate = issueDate,
            CustomerId = customerId,
            Status =status == null! ? new IssuedStatus():status
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