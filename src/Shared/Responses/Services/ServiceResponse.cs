using CleanArchTemplate.Shared.Responses.Routes;

namespace CleanArchTemplate.Shared.Responses.Services;

public class ServiceResponse
{
    public ServiceResponse()
    {
        
    }
    public ServiceResponse(Guid id, string name, decimal amount, string status, DateTime date, Guid routeId,bool deleted)
    {
        Id = id;
        Name = name;
        Amount = amount;
        Status = status;
        Date = date;
        RouteId = routeId;
        Deleted = deleted;
    }

    public Guid Id { get; set; }
    public string Name { get;  set; }
    public decimal Amount { get;  set; }
    public string Status { get;  set; }
    public DateTime Date { get;  set; }
    public Guid RouteId { get; set; }
    public bool Deleted { get; set; }
}

public class ServiceDefaultResponse
{
    public ServiceDefaultResponse()
    {

    }
    public ServiceDefaultResponse(IEnumerable<ServiceResponse> services, IEnumerable<RouteResponse> routes,IEnumerable<string> status)
    {
        Services = services;
        Routes = routes;
        Status = status;
    }

    public IEnumerable<string> Status { get; set; }

    public IEnumerable<ServiceResponse> Services { get; set; }
    public IEnumerable<RouteResponse> Routes { get; set; }
}
