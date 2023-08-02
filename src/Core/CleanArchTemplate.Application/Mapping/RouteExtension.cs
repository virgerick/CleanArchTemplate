using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
namespace CleanArchTemplate.Application.Mapping;
public static class MappingExtensions
{
    public static RouteResponse Map(this Route route)
    {
        return new RouteResponse(route.Id.Value, route.Origin, route.Destination, route.Amount);

    }
    public static IQueryable<RouteResponse> ProjectTo(this IQueryable<Route> source,int index)
    {
        if(source==null) throw new ArgumentNullException(nameof(source));
        return source.Select(route=>new RouteResponse(route.Id.Value, route.Origin, route.Destination,  route.Amount));
    }
}
