using FluentValidation;

namespace CleanArchTemplate.Domain.Invoices;

public class InvoiceValidator : AbstractValidator<Invoice>
{
    public InvoiceValidator()
    {
        RuleFor(invoice => invoice.IssueDate).NotNull();
        RuleFor(invoice => invoice.CustomerId).NotNull();
        

    }
}