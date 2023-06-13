
using CleanArchTemplate.Server.Endpoints.Models.Create;
using CleanArchTemplate.Server.Endpoints.Models.DeleteModel;
using CleanArchTemplate.Server.Endpoints.Models.EditModel;
using CleanArchTemplate.Server.Endpoints.Models.GetDefault;
using CleanArchTemplate.Server.Endpoints.Models.GetModel;
using CleanArchTemplate.Server.Endpoints.Models.GetModelById;

namespace CleanArchTemplate.Server.Endpoints.Models;
public class ModelEndpoints : IMapEndpoint
{
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup("/Model")
            .WithTags("Model");
        group.MapGetModelDefaultEndpoint();
        group.MapGetModelEndpoint();
        group.MapGetModelByIdEndpoint();
        group.MapCreateModelEndpoint();
        group.MapEditModelEndpoint();
        group.MapDeleteModelEndpoint();
        return group;
    }
}