namespace CleanArchTemplate.Domain.Invoices;

public record PaidStatus() : InvoiceStatus(InvoiceStatus.Paid);
