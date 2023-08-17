using CleanArchTemplate.Server.Endpoints;

namespace CleanArchTemplate.Server.Extensions;

public static class MapEndpointExtension{
    public static WebApplication UseMapEndpoint(this WebApplication app){
        var endpoints = GetMapEndpoints<IMapEndpoint>(typeof(IServerAssemblyMarkup));
        var group = app.MapGroup("api");
        foreach (var endpoint in endpoints)
        {
            endpoint.Map(group);      
        }
        return app;
    }
    private static IEnumerable<T> GetMapEndpoints<T>(Type scanningType) where T : IMapEndpoint
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && t is { IsAbstract: false, IsInterface: false })
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}