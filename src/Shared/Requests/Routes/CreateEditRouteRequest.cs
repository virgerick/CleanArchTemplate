namespace CleanArchTemplate.Shared.Requests.Routes;
public record struct CreateEditRouteRequest(string Origin, string Destination, float Distance, float EstimatedTime, decimal Amount, Guid VehicleId);