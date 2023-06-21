using CleanArchTemplate.Shared.Requests.Services;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

//interface of a service api service using refit
public interface IServiceApiService
{
    [Get("/Service/")]
    public Task<ResultList<ServiceResponse>> GetAsync(
        CancellationToken cancellationToken = default
    );

    [Get("/Service/Default")]
    public Task<Result<ServiceDefaultResponse>> GetDefaultAsync(
        CancellationToken cancellationToken = default
    );

    [Get("/Service/{Id}")]
    public Task<Result<ServiceResponse>> GetByIdAsync(
        Guid Id,
        CancellationToken cancellationToken = default
    );

    [Post("/Service/")]
    public Task<Result<Guid>> CreateAsync(
        [Body] AddEditServiceRequest request,
        CancellationToken cancellationToken = default
    );

    [Put("/Service/{Id}")]
    public Task<Result<Guid>> EditAsync(
        Guid Id,
        [Body] AddEditServiceRequest request,
        CancellationToken cancellationToken = default
    );

    [Delete("/Service/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}
