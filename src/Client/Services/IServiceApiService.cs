using CleanArchTemplate.Shared.Requests.Services;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

//interface of a service api service using refit
public interface IServiceApiService
{
    private const string EndPoint = "/Api/Services";
    [Get(EndPoint)]
    public Task<ResultList<ServiceResponse>> GetAsync(
        CancellationToken cancellationToken = default
    );

    [Get(EndPoint+"/Default")]
    public Task<Result<ServiceDefaultResponse>> GetDefaultAsync(
        CancellationToken cancellationToken = default
    );

    [Get(EndPoint+"/{Id}")]
    public Task<Result<ServiceResponse>> GetByIdAsync(
        Guid Id,
        CancellationToken cancellationToken = default
    );

    [Post(EndPoint+"/")]
    public Task<Result<Guid>> CreateAsync(
        [Body] AddEditServiceRequest request,
        CancellationToken cancellationToken = default
    );

    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(
        Guid Id,
        [Body] AddEditServiceRequest request,
        CancellationToken cancellationToken = default
    );

    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}
