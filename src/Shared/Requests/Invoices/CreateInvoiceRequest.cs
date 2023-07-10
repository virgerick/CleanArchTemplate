namespace CleanArchTemplate.Shared.Requests.Invoices;

public record CreateInvoiceRequest(Guid CustomerId,DateTime IssueDate,InvoiceLineRequest[] Lines);
public record InvoiceLineRequest(string Description,decimal Price,
    int Quantity = 1,Guid ServiceId = default,Guid RouteId = default);