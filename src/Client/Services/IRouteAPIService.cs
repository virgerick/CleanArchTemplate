using CleanArchTemplate.Shared.Requests.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;
public interface IRouteAPIService
{
    [Get("/Route/")]
    public Task<ResultList<RouteResponse>> GetAsync(CancellationToken cancellationToken = default);
    [Get("/Route/Default")]
    public Task<Result<RouteDefaultResponse>> GetDefaultAsync(CancellationToken cancellationToken = default);
    [Get("/Route/{Id}")]
    public Task<Result<RouteResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Route/")]
    public Task<Result<Guid>> CreateAsync([Body] CreateEditRouteRequest request, CancellationToken cancellationToken = default);
    [Put("/Route/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] CreateEditRouteRequest request, CancellationToken cancellationToken = default);
    [Delete("/Route/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    
}

