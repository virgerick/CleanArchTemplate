namespace CleanArchTemplate.Shared.Responses.Routes;
public class RouteResponse
{
    public Guid Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }
    public decimal Amount { get; set; }
    public Guid VehicleId { get; set; }
    public string Display => $"From {Origin} To {Destination}";
    public RouteResponse() { }
    public RouteResponse(Guid id, string origin, string destination, float distance, float estimatedTime, decimal amount, Guid vehicleId)
    {
        Id = id;
        Origin = origin;
        Destination = destination;
        Distance = distance;
        EstimatedTime = estimatedTime;
        Amount = amount;
        VehicleId = vehicleId;
    }
}
