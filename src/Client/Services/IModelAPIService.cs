using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IModelAPIService
{
    private const string EndPoint = "/Api/Models";
    [Get(EndPoint+"/Default")]
    public Task<Result<ModelDefaultResponse>> GetDefaultAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/")]
    public Task<ResultList<ModelResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/{Id}")]
    public Task<Result<ModelResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post(EndPoint+"/")]
    public Task<Result<Guid>> CreateAsync([Body] CreateEditModelRequest request, CancellationToken cancellationToken = default);
    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] CreateEditModelRequest request, CancellationToken cancellationToken = default);
    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}

