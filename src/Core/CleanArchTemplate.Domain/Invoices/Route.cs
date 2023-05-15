namespace CleanArchTemplate.Domain.Invoices;

public class Route
{
    public RouteId Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }

    public ICollection<Service> Services { get; set; }
}
