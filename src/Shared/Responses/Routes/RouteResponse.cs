
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Shared.Responses.Routes;
public record struct RouteResponse(Guid Id, string Origin, string Destination, float Distance, float EstimatedTime, decimal Amount, Guid VehicleId);
public record struct RouteDefaultResponse(IEnumerable<RouteResponse> Routes,IEnumerable<VehicleResponse> Vehicles,IEnumerable<string> Places);
