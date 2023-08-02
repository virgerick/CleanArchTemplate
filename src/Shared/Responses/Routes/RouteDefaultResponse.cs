
using CleanArchTemplate.Shared.Responses.Vehicles;

namespace CleanArchTemplate.Shared.Responses.Routes;

public record  RouteDefaultResponse(IEnumerable<RouteResponse> Routes,IEnumerable<string> Places);
