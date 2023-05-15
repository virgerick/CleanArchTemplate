namespace CleanArchTemplate.Domain.Invoices;

public record IssuedStatus() : InvoiceStatus(InvoiceStatus.Issued);
