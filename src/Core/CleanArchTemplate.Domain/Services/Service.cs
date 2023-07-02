using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain.Routes;
using OneOf;

namespace CleanArchTemplate.Domain.Services;
public record  ServiceId(Guid Value) {

    public static readonly ServiceId Empty = new(Guid.Empty);
    public static ServiceId NewId() => new(Guid.NewGuid());
};
public class Service:AuditableRootEntity<ServiceId>
{
    public static readonly Service Empty = new Service() {
        Id = ServiceId.Empty,
        Name = "NoService",
        Amount = 0,
        Status = new InactiveStatus()
    };
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public ServiceStatus Status { get; private set; }
    public ICollection<Customer> Customers { get; private set; } = new List<Customer>();
    public ICollection<InvoiceLine> InvoiceLines { get; private set; }=new List<InvoiceLine>();
    public ICollection<Driver> Drivers { get; private set; } = new List<Driver>();
    protected Service() { } 

    private Service(string name, decimal amount, ServiceStatus status)
    {
        Id=ServiceId.NewId();
        Name = name;
        Amount = amount;
        Status = status;
    }
    
    public static OneOf<Service,Exception> Create(string name,decimal amount)
    {
        if (string.IsNullOrWhiteSpace(name)) return new Exception("Name is required.");
        if (amount <= 0) return new Exception("Amount is required. and have to be grater than zero.");
        return new Service(name, amount, ServiceStatus.Active);

    }

    public void Update(string name,decimal newPrice)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
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
