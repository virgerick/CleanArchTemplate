using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Services;

namespace CleanArchTemplate.Domain.Routes;
public record RouteId(Guid Value);
public class Route:AuditableEntity<RouteId>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }
    public ICollection<Service> Services { get; set; }
}
