namespace CleanArchTemplate.Shared.Responses.Routes;
public class RouteResponse
{
    public Guid Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public decimal Amount { get; set; }
    public string Display => $"{Origin}/{Destination}";
    public RouteResponse() { }
    public RouteResponse(Guid id, string origin, string destination,  decimal amount)
    {
        Id = id;
        Origin = origin;
        Destination = destination;
        Amount = amount;
    }
}
