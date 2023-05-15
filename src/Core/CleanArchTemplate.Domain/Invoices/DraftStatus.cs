namespace CleanArchTemplate.Domain.Invoices;

public record DraftStatus() : InvoiceStatus(InvoiceStatus.Draft);
