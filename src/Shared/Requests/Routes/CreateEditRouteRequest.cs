namespace CleanArchTemplate.Shared.Requests.Routes;
public record  CreateEditRouteRequest(string Origin, string Destination, float Distance, float EstimatedTime, decimal Amount);