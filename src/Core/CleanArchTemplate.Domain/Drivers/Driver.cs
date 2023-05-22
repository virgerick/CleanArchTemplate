using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Services;

namespace CleanArchTemplate.Domain.Drivers;

public class Driver:AuditableEntity<DriverId>
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string License { get; set; }
    public DateTime HireDate { get; set; }
    public ICollection<Service> Services { get; set; }
}
public record struct DriverId(Guid Value);