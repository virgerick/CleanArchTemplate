using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Invoices;

namespace CleanArchTemplate.Domain.Routes;
public record struct RouteId(Guid Value) {
    public static RouteId NewId() => new RouteId(Guid.NewGuid());
};
public class Route:AuditableEntity<RouteId>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }
    public decimal Amount { get; set; }
    public VehicleId VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

}
