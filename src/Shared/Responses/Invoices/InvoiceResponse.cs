namespace CleanArchTemplate.Shared.Responses.Invoices;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime IssueDate { get; set; }
    public List<InvoiceLineResponse> Lines { get; set; }
}

public class InvoiceLineResponse
{
    public InvoiceLineResponse()
    {
        
    }
    public Guid Id { get; init; }
    public string Description{get;set;}
    public decimal Price{get;set;}
    public int Quantity{get;set;}
    public Guid ServiceId{get;set;}
    public Guid RouteId { get; set; }

    public static InvoiceLineResponse CreateFromService(string service, Guid serviceId, decimal price, int quantity) =>
        new InvoiceLineResponse()
        {
            Id = Guid.NewGuid(),
            Description = service,
            ServiceId = serviceId,
            Price = price,
            Quantity = quantity
        };

    public static InvoiceLineResponse CreateFromRoute(string route, Guid routeId, decimal price, int quantity) =>
        new() {  Id = Guid.NewGuid(),
            Description = route,
            RouteId = routeId,
            Price = price,
            Quantity = quantity };
}