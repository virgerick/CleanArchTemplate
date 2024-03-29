

using CleanArchTemplate.Server.Endpoints.Customers.Create;
using CleanArchTemplate.Server.Endpoints.Customers.DeleteCustomer;
using CleanArchTemplate.Server.Endpoints.Customers.Edit;
using CleanArchTemplate.Server.Endpoints.Customers.GetById;
using CleanArchTemplate.Server.Endpoints.Customers.GetCustomer;
using CleanArchTemplate.Server.Endpoints.Customers.Restore;

namespace CleanArchTemplate.Server.Endpoints.Customers;
public class CustomerEndpoints : IMapEndpoint
{
    private const string EndPoint = "Customers";
    public IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var group=endpoint.MapGroup(EndPoint)
            .WithTags(EndPoint);
        group.MapGetCustomerEndpoint();
        group.MapGetCustomerByIdEndpoint();
        group.MapCreateCustomerEndpoint();
        group.MapEditCustomerEndpoint();
        group.MapDeleteCustomerEndpoint();
        group.MapRestoreCustomerEndpoint();
        return group;
    }
}