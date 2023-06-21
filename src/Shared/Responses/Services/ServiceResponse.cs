using CleanArchTemplate.Shared.Responses.Routes;

namespace CleanArchTemplate.Shared.Responses.Services;

public class ServiceResponse
{
    public ServiceResponse() { }

    public ServiceResponse(
        Guid id,
        string name,
        string phoneNumber,
        string license,
        DateTime hireDate
    )
    {
        Id = id;
        Name = name;
        PhoneNumber = phoneNumber;
        License = license;
        HireDate = hireDate;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string License { get; set; }
    public DateTime HireDate { get; set; }
}

public class ServiceDefaultResponse
{
    public ServiceDefaultResponse()
    {

    }
    public ServiceDefaultResponse(IEnumerable<ServiceResponse> services, IEnumerable<RouteResponse> routes)
    {
        Services = services;
        Routes = routes;
    }

    public IEnumerable<ServiceResponse> Services { get; set; }
    public IEnumerable<RouteResponse> Routes { get; set; }
}
