using FluentValidation;

namespace CleanArchTemplate.Domain.Invoices;

public class InvoiceValidator : AbstractValidator<Invoice>
{
    public InvoiceValidator()
    {
        RuleFor(invoice => invoice.IssueDate).NotNull();
        RuleFor(invoice => invoice.CustomerId).NotNull();
        RuleFor(invoice => invoice.InvoiceLines)
            .NotNull()
            .NotEmpty()
            .WithMessage("Invoice must have at least one line.");

    }
}