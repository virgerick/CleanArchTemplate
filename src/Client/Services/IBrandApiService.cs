using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IBrandApiService
{
    private const string EndPoint = "/Api/Brands";
    [Get($"{EndPoint}")]
    public Task<ResultList<BrandResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get($"{EndPoint}/{{Id}}")]
    public Task<Result<BrandResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post($"{EndPoint}/")]
    public Task<Result<Guid>> CreateAsync([Body] CreateEditBrandRequest request, CancellationToken cancellationToken = default);
    [Put($"{EndPoint}/{{Id}}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] CreateEditBrandRequest request, CancellationToken cancellationToken = default);
    [Delete($"{EndPoint}/{{Id}}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}

