using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Server.Endpoints;

namespace CleanArchTemplate.Server.AutoRegistrar;

public class MapEndpointRegistrar:IRegistrarApplication
{
    public void Use(object App)
    {
       if(App is WebApplication app)
        {
            var endpoints = GetMapEndpoints<IMapEndpoint>(typeof(IServerAssemblyMarkup));
            foreach (var endpoint in endpoints)
            {
                endpoint.Map(app);      
            }
        }

    }
    private static IEnumerable<T> GetMapEndpoints<T>(Type scanningType) where T : IMapEndpoint
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }

}
