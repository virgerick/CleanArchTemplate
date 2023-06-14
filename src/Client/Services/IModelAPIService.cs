using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IModelAPIService
{
    [Get("/Model/Default")]
    public Task<Result<ModelDefaultResponse>> GetDefaultAsync(CancellationToken cancellationToken=default);
    [Get("/Model/")]
    public Task<ResultList<ModelResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/Model/{Id}")]
    public Task<Result<ModelResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Model/")]
    public Task<Result<Guid>> CreateAsync([Body] CreateEditModelRequest request, CancellationToken cancellationToken = default);
    [Put("/Model/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] CreateEditModelRequest request, CancellationToken cancellationToken = default);
    [Delete("/Model/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}

