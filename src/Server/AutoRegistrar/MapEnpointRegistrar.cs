using CleanArchTemplate.Application.Common.Interfaces.Common;
using CleanArchTemplate.Server.Endpoints;
using CleanArchTemplate.Server.Extensions;

namespace CleanArchTemplate.Server.AutoRegistrar;

public class MapEndpointRegistrar:IRegistrarApplication
{
    public void Use(object App)
    {
       if(App is WebApplication app)
        {
            app.UseMapEndpoint();  
        }

    }

}
