using CleanArchTemplate.Server.Endpoints;

namespace CleanArchTemplate.Server.Extensions;

public static class MapEndpointExtension{
    public static WebApplication UseMapEndpoint(this WebApplication app){
        var endpoints = GetMapEndpoints<IMapEndpoint>(typeof(IServerAssemblyMarkup));
        foreach (var endpoint in endpoints)
        {
            endpoint.Map(app);      
        }
        return app;
    }
    private static IEnumerable<T> GetMapEndpoints<T>(Type scanningType) where T : IMapEndpoint
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}