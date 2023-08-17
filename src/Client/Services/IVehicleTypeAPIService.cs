using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IVehicleTypeAPIService
{
    private const string EndPoint = "/Api/VehicleTypes";
    [Get(EndPoint)]
    public Task<ResultList<IdNameResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/{Id}")]
    public Task<Result<IdNameResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post(EndPoint)]
    public Task<Result<Guid>> CreateAsync([Body] NameRequest request, CancellationToken cancellationToken = default);
    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] NameRequest request, CancellationToken cancellationToken = default);
    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}