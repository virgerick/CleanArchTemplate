using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
namespace CleanArchTemplate.Application.Mapping;
public static class MappingExtensions
{
    public static RouteResponse Map(this Route route)
    {
        return new RouteResponse(route.Id.Value, route.Origin, route.Destination, route.Distance, route.EstimatedTime, route.Amount, route.VehicleId.Value);

    }
}
