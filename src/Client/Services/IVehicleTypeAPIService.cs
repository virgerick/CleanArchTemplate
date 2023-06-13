using CleanArchTemplate.Shared.Requests;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IVehicleTypeAPIService
{
    [Get("/VehicleType/")]
    public Task<ResultList<IdNameResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/VehicleType/{Id}")]
    public Task<Result<IdNameResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/VehicleType/")]
    public Task<Result<Guid>> CreateAsync([Body] NameRequest request, CancellationToken cancellationToken = default);
    [Put("/VehicleType/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] NameRequest request, CancellationToken cancellationToken = default);
    [Delete("/VehicleType/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}

public interface IBrandApiService
{
    [Get("/Brand/")]
    public Task<ResultList<BrandResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/Brand/{Id}")]
    public Task<Result<BrandResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Brand/")]
    public Task<Result<Guid>> CreateAsync([Body] CreateEditBrandRequest request, CancellationToken cancellationToken = default);
    [Put("/Brand/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body] CreateEditBrandRequest request, CancellationToken cancellationToken = default);
    [Delete("/Brand/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
}

