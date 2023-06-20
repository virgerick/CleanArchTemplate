
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Shared.Responses.Routes;

public record  RouteDefaultResponse(IEnumerable<RouteResponse> Routes,IEnumerable<VehicleResponse> Vehicles,IEnumerable<string> Places);
