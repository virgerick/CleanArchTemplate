using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Domain.Services;
using FluentValidation;

namespace CleanArchTemplate.Domain.Invoices;

public class InvoiceLineValidator : AbstractValidator<InvoiceLine>
{
    public InvoiceLineValidator()
    {
        RuleFor(invoiceLine => invoiceLine.Quantity).NotNull();
        
        RuleFor(invoiceLine => invoiceLine.ServiceId)
            .NotNull()
            .When(invoiceLine => invoiceLine.RouteId == RouteId.Empty);
        RuleFor(invoiceLine => invoiceLine.RouteId)
            .NotNull()
            .When(invoiceLine => invoiceLine.ServiceId == ServiceId.Empty);
    }
}