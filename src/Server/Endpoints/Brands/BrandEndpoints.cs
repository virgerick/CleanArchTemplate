
using CleanArchTemplate.Server.Endpoints.Brands.Create;
using CleanArchTemplate.Server.Endpoints.Brands.DeleteBrand;
using CleanArchTemplate.Server.Endpoints.Brands.EditBrand;
using CleanArchTemplate.Server.Endpoints.Brands.GetBrand;
using CleanArchTemplate.Server.Endpoints.Brands.GetBrandById;

namespace CleanArchTemplate.Server.Endpoints.Brands;
public class BrandEndpoints : IMapEndpoint
{
    private const string EndPoint = "Brands";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapGetBrandEndpoint();
        group.MapGetBrandByIdEndpoint();
        group.MapCreateBrandEndpoint();
        group.MapEditBrandEndpoint();
        group.MapDeleteBrandEndpoint();
        return group;
    }
}