

using CleanArchTemplate.Server.Endpoints.Services.Create;
using CleanArchTemplate.Server.Endpoints.Services.DeleteService;
using CleanArchTemplate.Server.Endpoints.Services.Edit;
using CleanArchTemplate.Server.Endpoints.Services.GetById;
using CleanArchTemplate.Server.Endpoints.Services.GetService;

namespace CleanArchTemplate.Server.Endpoints.Services;
public class ServiceEndpoints : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup("/Service")
            .WithTags("Service");
        group.MapGetServiceEndpoint();
        group.MapGetServiceDefaultEndpoint();
        group.MapGetServiceByIdEndpoint();
        group.MapCreateServiceEndpoint();
        group.MapEditServiceEndpoint();
        group.MapDeleteServiceEndpoint();
        return group;
    }
}