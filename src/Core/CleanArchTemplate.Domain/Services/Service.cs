using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using OneOf;

namespace CleanArchTemplate.Domain.Services;
public record struct ServiceId(Guid Value) {

    public static ServiceId NewId() => new(Guid.NewGuid());
};
public class Service:AuditableRootEntity<ServiceId>
{
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public ServiceStatus Status { get; private set; }
    public DateTime Date { get; private set; }
    public RouteId RouteId { get; set; }
    public Route Route { get; set; }
    public ICollection<Customer> Customers { get; private set; }
    public ICollection<InvoiceLine> InvoiceLines { get; private set; }
    public ICollection<Driver> Drivers { get; private set; }
    protected Service() { } // Constructor protegido para EF Core

    private Service(string name, decimal amount, ServiceStatus status, DateTime date, RouteId routeId)
    {
        Name = name;
        Amount = amount;
        Status = status;
        Date = date;
        RouteId = routeId;
    }
    
    public static OneOf<Service,Exception> Create(string name,decimal amount,DateTime date,RouteId routeId=default)
    {
        if (string.IsNullOrWhiteSpace(name)) return new Exception("Name is requiered.");
        if (amount <= 0) return new Exception("Amount is requiered. and have to be grater than zero.");
        return new Service(name, amount, ServiceStatus.InProgress, date, routeId);

    }

    public void Update(string name,decimal newPrice)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        if (newPrice <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));
        }

        Amount = newPrice;

    }

    public void Deactivate()
    {
        Status = new InactiveStatus();
    }
}
