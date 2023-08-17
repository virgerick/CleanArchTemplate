using CleanArchTemplate.Shared.Requests.Routes;
using CleanArchTemplate.Shared.Responses.Routes;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IRouteAPIService
{
    private const string EndPoint = "/Api/Routes";
    [Get(EndPoint)]
    public Task<ResultList<RouteResponse>> GetAsync(CancellationToken cancellationToken = default);

    [Get(EndPoint+"/Default")]
    public Task<Result<RouteDefaultResponse>> GetDefaultAsync(
        CancellationToken cancellationToken = default
    );

    [Get(EndPoint+"/{Id}")]
    public Task<Result<RouteResponse>> GetByIdAsync(
        Guid Id,
        CancellationToken cancellationToken = default
    );

    [Post(EndPoint+"/")]
    public Task<Result<Guid>> CreateAsync(
        [Body] CreateEditRouteRequest request,
        CancellationToken cancellationToken = default
    );

    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(
        Guid Id,
        [Body] CreateEditRouteRequest request,
        CancellationToken cancellationToken = default
    );

    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}
