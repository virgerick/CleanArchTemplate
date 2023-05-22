using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;

namespace CleanArchTemplate.Domain.Services;
public record struct ServiceId(Guid Value);
public class Service:AuditableRootEntity<ServiceId>
{
  
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public ServiceStatus Status { get; private set; }
    public ICollection<InvoiceLine> InvoiceLines { get; private set; }
    public RouteId RouteId { get; private set; }
    public Route Route { get; set; }
    protected Service() { } // Constructor protegido para EF Core

    public static Service Create(ServiceId id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Service name must not be empty.", nameof(name));
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.", nameof(price));
        }

        return new Service { Id = id, Name = name, Price = price, Status = new InProgressStatus() };
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));
        }

        Price = newPrice;
    }

    public void Deactivate()
    {
        Status = new InactiveStatus();
    }
}
